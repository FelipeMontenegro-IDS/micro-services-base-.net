using System.Text.Json;
using Application.Interfaces.Messaging;
using Azure.Core.Amqp;
using Azure.Messaging.ServiceBus;
using Shared.Configurations;
using Shared.Utils.Generals;

namespace Persistence.Messaging;

public class AzureServiceBusSender : IMessageSender
{
    private readonly ServiceBusClient _client;

    public AzureServiceBusSender(ServiceBusClient client)
    {
        _client = client;
    }

    public async Task SendMessageAsync<T>(T message, string queue,
        CancellationToken cancellationToken = default, 
        AzureProperties? properties = null)
    {
        var sender = _client.CreateSender(queue);
        var serializedMessage = JsonSerializer.Serialize(message);
        var serviceBusMessage = new ServiceBusMessage(serializedMessage);

        if (properties != null)
        {
            ValueAssignmentHelper.SetToStringIfNotNull(value => serviceBusMessage.MessageId = value,properties.MessageId );
            ValueAssignmentHelper.SetToStringIfNotNull(value => serviceBusMessage.CorrelationId = value,properties.CorrelationId );

        }

        await sender.SendMessageAsync(serviceBusMessage, cancellationToken);
    }
}