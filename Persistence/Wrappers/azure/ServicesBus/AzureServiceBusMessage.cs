using Application.Interfaces.Azure.ServicesBus;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Shared.Enums.Data;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Wrappers;

namespace Persistence.Wrappers.azure.ServicesBus;

/// <summary>
/// Clase que implementa la interfaz <see cref="IMessage"/> para interactuar con el servicio Azure Service Bus.
/// Proporciona métodos para enviar y recibir mensajes, manejar solicitudes y respuestas, y procesar mensajes de forma personalizada.
/// </summary>
/// <remarks>
/// Esta clase permite la integración completa con Azure Service Bus, gestionando las solicitudes y respuestas entre colas, 
/// además de soportar políticas de reintentos, cancelación y procesamiento flexible de mensajes.
/// </remarks>
public class AzureServiceBusMessage : IMessage
{
    private readonly IMessageSender _messageSender;
    private readonly IMessageReceiver _messageReceiver;
    private readonly IRetryPolicy _retryPolicy;
    private readonly ILogger<AzureServiceBusMessage> _receiverLogger;
    private readonly IValidationHelper _validationHelper;
    private readonly IServiceBusQueueTopicManager _queueTopicManager;

    public AzureServiceBusMessage(
        IMessageSender messageSender,
        IMessageReceiver messageReceiver,
        IRetryPolicy retryPolicy,
        ILogger<AzureServiceBusMessage> receiverLogger,
        IValidationHelper validationHelper,
        IServiceBusQueueTopicManager queueTopicManager)
    {
        _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
        _messageReceiver = messageReceiver ?? throw new ArgumentNullException(nameof(messageReceiver));
        _retryPolicy = retryPolicy ?? throw new ArgumentNullException(nameof(retryPolicy));
        _receiverLogger = receiverLogger ?? throw new ArgumentNullException(nameof(receiverLogger));
        _validationHelper = validationHelper ?? throw new ArgumentNullException(nameof(validationHelper));
        _queueTopicManager = queueTopicManager ?? throw new ArgumentNullException(nameof(queueTopicManager));
    }


    public async Task<TResponse> ProcessRequestAsync<TRequest, TResponse>(
        TRequest request,
        string queueRequest,
        string queueResponse,
        ServiceBusProcessorOptions options,
        CancellationToken cancellationToken = default,
        IDictionary<string, object>? headers = null,
        RetryPolicyDefault retryPolicyDefault = RetryPolicyDefault.NoRetries)
        where TRequest : class where TResponse : class
    {
        await _queueTopicManager.CreateQueueIfNotExists(queueRequest, cancellationToken);
        await _queueTopicManager.CreateTopicIfNotExists(queueResponse, cancellationToken);
        
        if (_validationHelper.IsNull(request)) throw new InvalidOperationException("The Request is null.");

        // Enviar solicitud
        await RetryAndSendMessageAsync(request, queueRequest, retryPolicyDefault, cancellationToken);

        TaskCompletionSource<TResponse> tcs = new TaskCompletionSource<TResponse>();

        // Crear una tarea para esperar la respuesta
        await _messageReceiver.RegisterMessageHandler<TResponse>(queueResponse, null, async (message, token) =>
        {
            try
            {
                if (_validationHelper.IsNull(request))
                    throw new InvalidOperationException("Error occurred while processing the message.");

                tcs.SetResult(message);
            }
            catch (Exception ex)
            {
                if (token.IsCancellationRequested)
                {
                    tcs.TrySetCanceled(token);
                }
                else
                {
                    tcs.TrySetException(ex);
                }
            }
        }, options, cancellationToken);

        return await tcs.Task;
    }

    public async Task ReceiveAndResponseAsync<TRequest, TResponse>(
        string queueRequest,
        string queueResponse,
        ServiceBusProcessorOptions options,
        Func<TRequest, CancellationToken, Task<TResponse>> processResponseToSend,
        CancellationToken cancellationToken = default,
        IDictionary<string, object>? headers = null,
        RetryPolicyDefault retryPolicyDefault = RetryPolicyDefault.NoRetries)
        where TRequest : class where TResponse : class
    {
        await _queueTopicManager.CreateQueueIfNotExists(queueRequest, cancellationToken);
        await _queueTopicManager.CreateTopicIfNotExists(queueResponse, cancellationToken);
        
        await _messageReceiver.RegisterMessageHandler<TRequest>(queueRequest, null,
            async (message, token) =>
            {
                try
                {
                    if (_validationHelper.IsNull(message))
                        throw new InvalidOperationException("Error occurred while processing the request.");

                    TResponse responseMessage = await processResponseToSend(message, token);

                    await RetryAndSendMessageAsync(
                        responseMessage,
                        queueResponse,
                        retryPolicyDefault,
                        cancellationToken);
                }
                catch (Exception ex)
                {
                    _receiverLogger.LogError($"Error while processing message: {ex.Message}");
                }
            }, options, cancellationToken);
    }

    public async Task SendAsync<T>(
        T message,
        string queue,
        IDictionary<string, object>? headers = null,
        CancellationToken cancellationToken = default,
        RetryPolicyDefault retryPolicyDefault = RetryPolicyDefault.NoRetries
    ) where T : class
    {
        if (_validationHelper.IsNull(message)) throw new InvalidOperationException("The message is null.");
        
        await RetryAndSendMessageAsync(message, queue, retryPolicyDefault, cancellationToken);
    }

    public async Task<T> ProcessAndReturnMessageAsync<T>(
        string queueOrTopicName,
        string? subscriptionName,
        Func<T, CancellationToken, Task<T>> processMessageAsync,
        ServiceBusProcessorOptions options,
        CancellationToken cancellationToken = default) where T : class
    {

        var tcs = new TaskCompletionSource<T>();

        await _messageReceiver.RegisterMessageHandler<T>(queueOrTopicName, subscriptionName, async (message, token) =>
        {
            if (_validationHelper.IsNull(message))
            {
                tcs.TrySetException(new InvalidOperationException("Error occurred while processing the message."));
                return;
            }

            try
            {
                var processedMessage = await processMessageAsync(message, token);
                tcs.TrySetResult(processedMessage);
            }
            catch (Exception ex)
            {
                _receiverLogger.LogError($"Error while processing message: {ex.Message}");
                tcs.TrySetException(ex);
                throw;
            }
        }, options, cancellationToken);

        return await tcs.Task;
    }


    #region Private Method's

    private async Task RetryAndSendMessageAsync<T>(T message, string queue, RetryPolicyDefault retryPolicyDefault,
        CancellationToken cancellationToken)
    {
        try
        {
            await _queueTopicManager.CreateQueueIfNotExists(queue, cancellationToken);

            await _retryPolicy.RetryPolicyAsync(
                async () => await _messageSender.SendMessageAsync(message, queue, cancellationToken),
                retryPolicyDefault, cancellationToken);
        }
        catch (Exception ex)
        {
            // Aquí puedes registrar el error o tomar alguna otra acción
            throw new InvalidOperationException("Failed to send message after retries.", ex);
        }
    }

    #endregion
}