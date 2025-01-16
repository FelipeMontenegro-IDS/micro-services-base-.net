using System.Text.Json;
using Application.Interfaces.Azure.ServicesBus;
using Azure.Messaging.ServiceBus;
using Shared.Configurations;
using Shared.Constants;
using Shared.Helpers;
using Shared.Enums;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers;

namespace Persistence.Wrappers.azure.ServicesBus;

public class AzureServiceBusSender : IMessageSender
{
    private readonly ServiceBusClient _client;
    private readonly IDateTimeHelper _dateTimeHelper;
    private readonly IContentTypeProvider _contentTypeProvider;
    private readonly IValidationHelper _validationHelper;
    private readonly IValueAssignmentHelper _valueAssignmentHelper;

    public AzureServiceBusSender(
        ServiceBusClient client,
        IDateTimeHelper dateTimeHelper,
        IContentTypeProvider contentTypeProvider,
        IValidationHelper validationHelper,
        IValueAssignmentHelper valueAssignmentHelper)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _dateTimeHelper = dateTimeHelper ?? throw new ArgumentNullException(nameof(dateTimeHelper));
        _contentTypeProvider = contentTypeProvider ?? throw new ArgumentNullException(nameof(contentTypeProvider));
        _validationHelper = validationHelper ?? throw new ArgumentNullException(nameof(validationHelper));
        _valueAssignmentHelper = valueAssignmentHelper ?? throw new ArgumentNullException(nameof(valueAssignmentHelper));
    }

    public async Task SendMessageAsync<T>(T message, string queue,
        CancellationToken cancellationToken = default,
        AzureProperty? properties = null)
    {
        var sender = _client.CreateSender(queue);
        var serializedMessage = JsonSerializer.Serialize(message);
        var serviceBusMessage = new ServiceBusMessage(serializedMessage);

        if (_validationHelper.IsNotNull(properties))
        {
            _valueAssignmentHelper.SetToStringIfNotNull(value => serviceBusMessage.MessageId = value, properties.MessageId);
            _valueAssignmentHelper.SetToStringIfNotNull(value => serviceBusMessage.CorrelationId = value, properties.CorrelationId);
            _valueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.Subject = val, properties.Subject);
            _valueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.ReplyTo = val, properties.ReplyTo);
            _valueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.To = val, properties.To);
            _valueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.ReplyToSessionId = val, properties.ReplyToSessionId);
            _valueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.PartitionKey = val, properties.PartitionKey);
            _valueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.SessionId = val, properties.SessionId);
            _valueAssignmentHelper.SetIfNotNull(val => serviceBusMessage.Body = val, properties.Body);
            _valueAssignmentHelper.SetIfNotNullOrEmpty(val => serviceBusMessage.TransactionPartitionKey = val, properties.TransactionPartitionKey);
            _valueAssignmentHelper.SetIf(val => serviceBusMessage.TimeToLive = val, properties.TimeToLive, ttl => ttl > TimeSpan.Zero);
            _valueAssignmentHelper.SetIf(val => serviceBusMessage.ScheduledEnqueueTime = val, properties.ScheduledEnqueueTimeUtc, dt => dt > _dateTimeHelper.GetCurrentUtcDateTime());
            _valueAssignmentHelper.SetDefaultIfNull(val => serviceBusMessage.ContentType = val, null, _contentTypeProvider.GetValue(ContentType.ApplicationJson, ContentTypeConstant.Json));
        }

        await sender.SendMessageAsync(serviceBusMessage, cancellationToken);
    }
}