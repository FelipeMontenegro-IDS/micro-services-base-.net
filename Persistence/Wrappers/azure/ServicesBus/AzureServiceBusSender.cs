using System.Text.Json;
using Application.Interfaces.Azure.ServicesBus;
using Azure.Messaging.ServiceBus;
using Shared.Configurations;
using Shared.Utils.Enums;
using Shared.Utils.Helpers;
using Shared.Utils.Providers;

namespace Persistence.Wrappers.azure.ServicesBus;

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

        if (ValidationHelper.IsNotNull(properties))
        {
            ValueAssignmentHelper.SetToStringIfNotNull(value => serviceBusMessage.MessageId = value, properties.MessageId);
            ValueAssignmentHelper.SetToStringIfNotNull(value => serviceBusMessage.CorrelationId = value, properties.CorrelationId);
            ValueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.Subject = val, properties.Subject);
            ValueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.ReplyTo = val, properties.ReplyTo);
            ValueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.To = val, properties.To);
            ValueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.ReplyToSessionId = val, properties.ReplyToSessionId);
            ValueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.PartitionKey = val, properties.PartitionKey);
            ValueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.SessionId = val, properties.SessionId);
            ValueAssignmentHelper.SetIfNotNull(val => serviceBusMessage.Body = val, properties.Body);
            ValueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.TransactionPartitionKey = val, properties.TransactionPartitionKey);
            ValueAssignmentHelper.SetIf(val => serviceBusMessage.TimeToLive = val, properties.TimeToLive, ttl => ttl > TimeSpan.Zero);
            ValueAssignmentHelper.SetIf(val => serviceBusMessage.ScheduledEnqueueTime = val, properties.ScheduledEnqueueTimeUtc, dt => dt > DateTimeHelper.GetCurrentUtcDateTime());
            ValueAssignmentHelper.SetDefaultIfNull(val => serviceBusMessage.ContentType = val, null, ContentTypeProvider.GetContentType(ContentType.ApplicationJson));
        }

        await sender.SendMessageAsync(serviceBusMessage, cancellationToken);
    }
}