using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Domain.Messaging;

namespace Persistence.Messaging;

public class AzureServiceBusSender : IMessageSender
{
    private readonly ServiceBusClient _client;

    public AzureServiceBusSender(ServiceBusClient client)
    {
        _client = client;
    }

    public async Task SendMessageAsync<TRequest>(TRequest message, string queueOrTopicName)
    {
        var sender = _client.CreateSender(queueOrTopicName);
        var serializedMessage = JsonSerializer.Serialize(message);
        var serviceBusMessage = new ServiceBusMessage(serializedMessage);

        await sender.SendMessageAsync(serviceBusMessage);    
    }
}