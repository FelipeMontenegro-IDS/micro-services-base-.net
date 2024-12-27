using Azure.Messaging.ServiceBus;
using Application.Interfaces.Messaging;
using Persistence.Wrappers;
using Shared.Utils.Enums;

namespace Persistence.Messaging;

public class AzureServiceBusMessage : IMessageService
{
    private readonly IMessageSender _messageSender;
    private readonly IMessageReceiver _messageReceiver;
    private readonly IMessageRetryPolicy _retryPolicy;

    public AzureServiceBusMessage(IMessageSender messageSender, IMessageReceiver messageReceiver,
        IMessageRetryPolicy retryPolicy)
    {
        _messageSender = messageSender;
        _messageReceiver = messageReceiver;
        _retryPolicy = retryPolicy;
    }


    public async Task<TResponse> ProcessRequestAsync<TRequest, TResponse>(
        TRequest request,
        string queueRequest,
        string queueResponse,
        ServiceBusProcessorOptions options,
        CancellationToken cancellationToken = default,
        IDictionary<string, object>? headers = null,
        RetryPolicyDefaults retryPolicyDefaults = RetryPolicyDefaults.NoRetries) where TRequest : class where TResponse : class
    {
        if (request == null) throw new InvalidOperationException("The Request is null.");
        // Enviar solicitud
        

        await RetryAndSendMessageAsync(request, queueRequest, retryPolicyDefaults, cancellationToken);

        var tcs = new TaskCompletionSource<TResponse>();
    
        // Crear una tarea para esperar la respuesta
        await _messageReceiver.RegisterMessageHandler<ProcessMessageEventArgs>(queueResponse, async (args, token) =>
        {
            try
            {
                // Deserializar el mensaje de respuesta a tipo TResponse
                var consumeContext = new ConsumeContext<TResponse>(args);
                var message = consumeContext.Message;
                if (message == null)
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

    public async Task<TResponse> ReceiveAndResponseAsync<TRequest, TResponse>(
        string queueRequest,
        string queueResponse,
        ServiceBusProcessorOptions options,
        Func<TRequest, CancellationToken, Task<TResponse>> processResponseToSend,
        CancellationToken cancellationToken = default,
        IDictionary<string, object>? headers = null,
        RetryPolicyDefaults retryPolicyDefaults = RetryPolicyDefaults.NoRetries)
        where TRequest : class where TResponse : class
    {
        var tcs = new TaskCompletionSource<TResponse>();

        await _messageReceiver.RegisterMessageHandler<ProcessMessageEventArgs>(queueRequest, async (args, token) =>
        {
            try
            {
                var consumeContext = new ConsumeContext<TRequest>(args);
                var message = consumeContext.Message;

                if (message == null)
                    throw new InvalidOperationException("Error occurred while processing the request.");

                TResponse responseMessage = await processResponseToSend(message, token);

                await RetryAndSendMessageAsync(responseMessage, queueRequest, retryPolicyDefaults, cancellationToken);

                await consumeContext.CompleteAsync(cancellationToken);

                tcs.SetResult(responseMessage);
            }
            catch (Exception e)
            {
                if (args.CancellationToken.IsCancellationRequested)
                {
                    tcs.TrySetCanceled(args.CancellationToken);
                }
                else
                {
                    tcs.TrySetException(e);
                }
            }
        }, options, cancellationToken);
        return await tcs.Task;  
    }

    public async Task SendAsync<T>(
        T message,
        string queue,
        IDictionary<string, object>? headers = null,
        CancellationToken cancellationToken = default,
        RetryPolicyDefaults retryPolicyDefaults = RetryPolicyDefaults.NoRetries
    )
    {
        if (message == null) throw new InvalidOperationException("The message is null.");

        await RetryAndSendMessageAsync(message, queue, retryPolicyDefaults, cancellationToken);
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