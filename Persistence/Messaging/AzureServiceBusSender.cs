using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Domain.Messaging;

namespace Persistence.Messaging;

public class AzureServiceBusSender<T> : IMessageSender<T>
{
    private readonly ServiceBusClient _client;

    public AzureServiceBusSender(ServiceBusClient client)
    {
        _client = client;
    }

    public async Task SendMessageAsync(T message, string queueOrTopicName)
    {
        var sender = _client.CreateSender(queueOrTopicName);
        var serializedMessage = JsonSerializer.Serialize(message);
        var serviceBusMessage = new ServiceBusMessage(serializedMessage);

        await sender.SendMessageAsync(serviceBusMessage);
    }
}