using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces.Azure.ServicesBus;
using Shared.Helpers;
using Shared.Enums;
using Shared.Interfaces.RetryPolicy;

namespace Persistence.Wrappers.azure.ServicesBus;

public class AzureServiceBusMessage : IMessage
{
    private readonly IMessageSender _messageSender;
    private readonly IMessageReceiver _messageReceiver;
    private readonly IRetryPolicy _retryPolicy;
    private readonly ILogger<AzureServiceBusMessage> _receiverLogger;

    public AzureServiceBusMessage(IMessageSender messageSender, IMessageReceiver messageReceiver,
        IRetryPolicy retryPolicy, ILogger<AzureServiceBusMessage> receiverLogger)
    {
        _messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
        _messageReceiver = messageReceiver ?? throw new ArgumentNullException(nameof(messageReceiver));
        _retryPolicy = retryPolicy ?? throw new ArgumentNullException(nameof(retryPolicy));
        _receiverLogger = receiverLogger ?? throw new ArgumentNullException(nameof(receiverLogger));
    }


    public async Task<TResponse> ProcessRequestAsync<TRequest, TResponse>(
        TRequest request,
        string queueRequest,
        string queueResponse,
        ServiceBusProcessorOptions options,
        CancellationToken cancellationToken = default,
        IDictionary<string, object>? headers = null,
        RetryPolicyDefaults retryPolicyDefaults = RetryPolicyDefaults.NoRetries)
        where TRequest : class where TResponse : class
    {
        if (ValidationHelper.IsNull(request)) throw new InvalidOperationException("The Request is null.");
        // Enviar solicitud

        await RetryAndSendMessageAsync(request, queueRequest, retryPolicyDefaults, cancellationToken);

        var tcs = new TaskCompletionSource<TResponse>();

        // Crear una tarea para esperar la respuesta
        await _messageReceiver.RegisterMessageHandler<TResponse>(queueResponse, async (message, token) =>
        {
            try
            {
                if (ValidationHelper.IsNull(request))
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

        // Esperar y devolver la respuesta
        return await tcs.Task;
    }

    public async Task ReceiveAndResponseAsync<TRequest, TResponse>(
        string queueRequest,
        string queueResponse,
        ServiceBusProcessorOptions options,
        Func<TRequest, CancellationToken, Task<TResponse>> processResponseToSend,
        CancellationToken cancellationToken = default,
        IDictionary<string, object>? headers = null,
        RetryPolicyDefaults retryPolicyDefaults = RetryPolicyDefaults.NoRetries)
        where TRequest : class where TResponse : class
    {
        await _messageReceiver.RegisterMessageHandler<TRequest>(queueRequest, async (message, token) =>
        {
            try
            {
                if (ValidationHelper.IsNull(message))
                    throw new InvalidOperationException("Error occurred while processing the request.");

                TResponse responseMessage = await processResponseToSend(message, token);

                await RetryAndSendMessageAsync(responseMessage, queueResponse, retryPolicyDefaults,
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
        RetryPolicyDefaults retryPolicyDefaults = RetryPolicyDefaults.NoRetries
    ) where T : class
    {
        if (ValidationHelper.IsNull(message)) throw new InvalidOperationException("The message is null.");

        await RetryAndSendMessageAsync(message, queue, retryPolicyDefaults, cancellationToken);
    }

    public async Task<T> ProcessAndReturnMessageAsync<T>(
        string queue,
        Func<T, CancellationToken, Task<T>> processMessage,
        ServiceBusProcessorOptions options,
        CancellationToken cancellationToken = default) where T : class
    {
        var tcs = new TaskCompletionSource<T>();

        await _messageReceiver.RegisterMessageHandler<T>(queue, async (message, token) =>
        {
            if (ValidationHelper.IsNull(message))
            {
                tcs.TrySetException(new InvalidOperationException("Error occurred while processing the message."));
                return;
            }

            try
            {
                var processedMessage = await processMessage(message, token);
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

    private async Task RetryAndSendMessageAsync<T>(T message, string queue, RetryPolicyDefaults retryPolicyDefaults,
        CancellationToken cancellationToken)
    {
        try
        {
            await _retryPolicy.RetryPolicyAsync(
                async () => await _messageSender.SendMessageAsync(message, queue, cancellationToken),
                retryPolicyDefaults, cancellationToken);
        }
        catch (Exception ex)
        {
            // Aquí puedes registrar el error o tomar alguna otra acción
            throw new InvalidOperationException("Failed to send message after retries.", ex);
        }
    }

    #endregion
}