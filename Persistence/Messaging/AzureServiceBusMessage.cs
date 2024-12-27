using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Application.Interfaces.Messaging;
using Persistence.Wrappers;

namespace Persistence.Messaging;

public class AzureServiceBusMessage : IMessageService
{
    private readonly IMessageSender _messageSender;
    private readonly IMessageReceiver _messageReceiver;

    public AzureServiceBusMessage(IMessageSender messageSender, IMessageReceiver messageReceiver)
    {
        _messageSender = messageSender;
        _messageReceiver = messageReceiver;
    }


    public async Task<TResponse> ProcessRequestAsync<TRequest, TResponse>(TRequest request, string queueRequest,
        string queueResponse, CancellationToken cancellationToken = default) where TRequest : class where TResponse : class
    {
        if (request == null) throw new InvalidOperationException("The Request is null.");
        // Enviar solicitud
        await _messageSender.SendMessageAsync(request, queueRequest, cancellationToken);

        var tcs = new TaskCompletionSource<TResponse>();

        // Crear una tarea para esperar la respuesta
        await _messageReceiver.RegisterMessageHandler<ProcessMessageEventArgs>(queueResponse, async (args, token) =>
        {
            try
            {
                // Deserializar el mensaje de respuesta a tipo TResponse
                var consumeContext = new ConsumeContext<ProcessMessageEventArgs>(args);
                var message = consumeContext.Message;
                if (message == null)
                    throw new InvalidOperationException("Error occurred while processing the message.");
                tcs.SetResult(null!);
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
        }, new ServiceBusProcessorOptions { AutoCompleteMessages = false }, cancellationToken);

        // Esperar y devolver la respuesta
        return await tcs.Task;
    }

    public async Task<TResponse> ReceiveAndResponseAsync<TRequest, TResponse>(string queueRequest, string queueResponse,
        Func<TRequest, CancellationToken, Task<TResponse>> processResponseToSend,
        CancellationToken cancellationToken = default) where TRequest : class where TResponse : class
    {
        var tcs = new TaskCompletionSource<TResponse>();

        await _messageReceiver.RegisterMessageHandler<ProcessMessageEventArgs>(queueRequest, async (args, token) =>
        {
            var consumeContext = new ConsumeContext<ProcessMessageEventArgs>(args);
            var message = consumeContext.Message;

            if (message == null)
                throw new InvalidOperationException("Error occurred while processing the request.");

            TResponse responseMessage = await processResponseToSend(null!, token);

            await _messageSender.SendMessageAsync(message, queueResponse, cancellationToken);
            tcs.SetResult(responseMessage);
            
        }, new ServiceBusProcessorOptions { AutoCompleteMessages = false }, cancellationToken);
        return await tcs.Task;
    }
}