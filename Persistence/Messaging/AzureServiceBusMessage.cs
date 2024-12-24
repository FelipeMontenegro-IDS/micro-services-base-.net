using System.Text.Json;
using Domain.Messaging;

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


    public async Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request, string queueRequest,
        string queueResponse)
    {
        if (request == null) throw new InvalidOperationException("The Request is null.");
        // Enviar solicitud
        await _messageSender.SendMessageAsync(request, queueRequest);

        var tcs = new TaskCompletionSource<TResponse>();

        // Crear una tarea para esperar la respuesta
        await _messageReceiver.RegisterMessageHandler<TResponse>(queueResponse, async response =>
        {
            // Deserializar el mensaje de respuesta a tipo TResponse
            var responseMessage =
                JsonSerializer.Deserialize<TResponse>(response.ToString() ?? throw new InvalidOperationException());
            if (responseMessage == null)
                throw new InvalidOperationException("Error occurred while processing the message.");
            tcs.SetResult(responseMessage);
        });

        // Esperar y devolver la respuesta
        return await tcs.Task;
    }
}