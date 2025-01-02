using System.Text.Json;
using Application.Interfaces.Messaging;
using Azure.Messaging.ServiceBus;
using Shared.Configurations;
using Shared.Utils.Helpers;

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
            ValueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.ContentType = val, properties.ContentType);
            ValueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.Subject = val, properties.Subject);
            ValueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.ReplyTo = val, properties.ReplyTo);
            ValueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.To = val, properties.To);
            ValueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.ReplyToSessionId = val, properties.ReplyToSessionId);
            ValueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.PartitionKey = val, properties.PartitionKey);
            ValueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.SessionId = val, properties.SessionId);
            ValueAssignmentHelper.SetIf(val => serviceBusMessage.TimeToLive = val, properties.TimeToLive, ttl => ttl > TimeSpan.Zero);
            ValueAssignmentHelper.SetIf(val => serviceBusMessage.ScheduledEnqueueTime = val, properties.ScheduledEnqueueTimeUtc, dt => dt > DateTimeHelper.GetCurrentUtcDateTime());
        }

        await sender.SendMessageAsync(serviceBusMessage, cancellationToken);
    }
}