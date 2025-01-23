using Application.Interfaces.Azure.ServicesBus;
using Azure;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using Shared.Constants.Data;
using Shared.Converters;
using Shared.Enums.Data;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Data;
using Shared.RegularExpressions;

namespace Persistence.Wrappers.azure.ServicesBus;

/// <summary>
/// Clase que gestiona la creación y validación de colas en Azure Service Bus.
/// </summary>
/// <remarks>
/// La clase <c>ServiceBusQueueManager</c> proporciona métodos para crear colas en Azure Service Bus
/// y validar los nombres de las colas. Utiliza el cliente de administración de Service Bus para
/// interactuar con el servicio y asegura que las colas se configuren correctamente según las
/// especificaciones dadas.
/// </remarks>
public class ServiceBusQueueTopicManager : IServiceBusQueueTopicManager
{
    private readonly ServiceBusAdministrationClient _adminClient;
    private readonly ILogger<ServiceBusQueueTopicManager> _logger;
    private readonly ITimeSpanHelper _timeSpanHelper;
    private readonly IFileSizeProvider _fileSizeProvider;
    private readonly FileSizeConverter _fileSizeConverter;
    private readonly IValidationHelper _validationHelper;

    public ServiceBusQueueTopicManager(
        ServiceBusAdministrationClient adminClient,
        ILogger<ServiceBusQueueTopicManager> logger,
        ITimeSpanHelper timeSpanHelper,
        IFileSizeProvider fileSizeProvider,
        FileSizeConverter fileSizeConverter,
        IValidationHelper validationHelper)
    {
        _adminClient = adminClient ?? throw new ArgumentNullException(nameof(adminClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _timeSpanHelper = timeSpanHelper ?? throw new ArgumentNullException(nameof(timeSpanHelper));
        _fileSizeProvider = fileSizeProvider ?? throw new ArgumentNullException(nameof(fileSizeProvider));
        _fileSizeConverter = fileSizeConverter ?? throw new ArgumentNullException(nameof(fileSizeConverter));
        _validationHelper = validationHelper ?? throw new ArgumentNullException(nameof(validationHelper));
    }

    public async Task CreateQueueIfNotExists(string queueName, CancellationToken cancellationToken = default)
    {
        if (!IsValidQueueName(queueName))
            throw new ArgumentException(
                $"The queue name {queueName} is invalid." +
                $"It must contain only lowercase letters, numbers, dots (.), " +
                $"hyphens (-), underscores (_) and forward slashes (/), " +
                $"and be up to 260 characters long.",
                nameof(queueName));

        if (!await _adminClient.QueueExistsAsync(queueName, cancellationToken))
        {
            var options = new CreateQueueOptions(queueName)
            {
                Name = queueName,
                DefaultMessageTimeToLive = _timeSpanHelper.CreateTimeSpanFromDays(15),
                RequiresDuplicateDetection = true,
                EnableBatchedOperations = true,
                DuplicateDetectionHistoryTimeWindow = _timeSpanHelper.CreateTimeSpanFromDays(15),
                MaxDeliveryCount = 1000,
                MaxSizeInMegabytes = _fileSizeConverter.Convert(
                    _fileSizeProvider.GetValue(FileSize.Gb1, FileSizeConstant.Gb1),
                    FileSizeUnit.Bytes, FileSizeUnit.Megabytes)
            };

            await _adminClient.CreateQueueAsync(options, cancellationToken);
            _logger.LogInformation($"Created queue {queueName}");
        }
    }

    public bool IsValidQueueName(string queueName)
    {
        if (string.IsNullOrWhiteSpace(queueName))
            throw new ArgumentException("The queue name cannot be null or empty.");

        return RegularExpression.NameQueue.IsMatch(queueName);
    }

    public async Task CreateTopicIfNotExists(string topicName, CancellationToken cancellationToken = default)
    {
        if (!IsValidTopicName(topicName))
            throw new ArgumentException(
                $"The topic name {topicName} is invalid." +
                $"It must contain only lowercase letters, numbers, dots (.), " +
                $"hyphens (-), underscores (_) and forward slashes (/), " +
                $"and be up to 260 characters long.",
                nameof(topicName));

        if (!await _adminClient.TopicExistsAsync(topicName, cancellationToken))
        {
            var options = new CreateTopicOptions(topicName)
            {
                Name = topicName,
                DefaultMessageTimeToLive = _timeSpanHelper.CreateTimeSpanFromDays(15),
                MaxSizeInMegabytes = _fileSizeConverter.Convert(
                    _fileSizeProvider.GetValue(FileSize.Gb1, FileSizeConstant.Gb1),
                    FileSizeUnit.Bytes, FileSizeUnit.Megabytes),
                EnableBatchedOperations = true
            };

            await _adminClient.CreateTopicAsync(options, cancellationToken);
            _logger.LogInformation($"Created topic {topicName}");
        }
    }

    public bool IsValidTopicName(string topicName)
    {
        if (string.IsNullOrWhiteSpace(topicName))
            throw new ArgumentException("The topic name cannot be null or empty.");

        return RegularExpression.NameQueue.IsMatch(topicName);
    }

    public async Task<bool> QueueExists(string queueName, CancellationToken cancellationToken = default)
    {
        try
        {
            Response<QueueProperties>? queue = await _adminClient.GetQueueAsync(queueName,cancellationToken);
            return _validationHelper.IsNotNull(queue);
        }
        catch (ServiceBusException ex) when (ex.Reason == ServiceBusFailureReason.MessagingEntityNotFound)
        {
            return false;
        }
    }

    public async Task<bool> TopicExists(string topicName,CancellationToken cancellationToken = default)
    {
        try
        {
            Response<TopicProperties>? topic = await _adminClient.GetTopicAsync(topicName,cancellationToken);
            return _validationHelper.IsNotNull(topic);
        }
        catch (ServiceBusException ex) when (ex.Reason == ServiceBusFailureReason.MessagingEntityNotFound)
        {
            return false;
        }
    }

    public async Task CreateSubscriptionIfNotExists(string topicName, string subscriptionName,CancellationToken cancellationToken = default)
    {
        try
        {
            // Intentar crear la suscripción si no existe
            await _adminClient.CreateSubscriptionAsync(topicName, subscriptionName, cancellationToken);
        }
        catch (ServiceBusException ex) when (ex.Reason == ServiceBusFailureReason.MessagingEntityAlreadyExists)
        {
            // Si la suscripción ya existe, no hacemos nada
        }
    }

    public async Task<bool> IsTopic(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            // Intentamos obtener el tópico usando el cliente de administración
            var topic = await _adminClient.GetTopicAsync(name, cancellationToken);
            return _validationHelper.IsNotNull(topic); // Si encontramos el tópico, significa que existe
        }
        catch (ServiceBusException ex) when (ex.Reason == ServiceBusFailureReason.MessagingEntityNotFound)
        {
            // Si la entidad no se encuentra, asumimos que no es un tópico
            return false;
        }
    }
}