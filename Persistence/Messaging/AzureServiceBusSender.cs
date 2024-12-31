using System.Text.Json;
using Application.Interfaces.Messaging;
using Azure.Core.Amqp;
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
        // AmqpMessageId amqpMessageId = new AmqpMessageId(serializedMessage);
        var serviceBusMessage = new ServiceBusMessage(serializedMessage);
        // serviceBusMessage.MessageId = "";
            

        

        await sender.SendMessageAsync(serviceBusMessage, cancellationToken);
    }
}