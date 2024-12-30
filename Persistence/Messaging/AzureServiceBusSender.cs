using System.Text.Json;
using Application.Interfaces.Messaging;
using Azure.Messaging.ServiceBus;

namespace Persistence.Messaging;

public class AzureServiceBusSender : IMessageSender
{
    private readonly ServiceBusClient _client;

    public AzureServiceBusSender(ServiceBusClient client)
    {
        _client = client;
    }

    public async Task SendMessageAsync<T>(T message, string queue,
        CancellationToken cancellationToken = default)
    {
        var sender = _client.CreateSender(queue);
        var serializedMessage = JsonSerializer.Serialize(message);
        var serviceBusMessage = new ServiceBusMessage(serializedMessage);

        await sender.SendMessageAsync(serviceBusMessage, cancellationToken);
    }
}