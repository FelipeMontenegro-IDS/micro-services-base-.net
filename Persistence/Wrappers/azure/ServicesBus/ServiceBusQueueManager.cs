using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces.Azure.ServicesBus;
using Shared.Converters;
using Shared.Enums;
using Shared.Helpers;
using Shared.Providers;
using Shared.RegularExpressions;

namespace Persistence.Wrappers.azure.ServicesBus;

public class ServiceBusQueueManager : IServiceBusQueueManager
{
    private readonly ServiceBusAdministrationClient _adminClient;
    private readonly ILogger<ServiceBusQueueManager> _logger;

    public ServiceBusQueueManager(ServiceBusAdministrationClient adminClient, ILogger<ServiceBusQueueManager> logger)
    {
        _adminClient = adminClient;
        _logger = logger;
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
                DefaultMessageTimeToLive = TimeSpanHelper.CreateTimeSpanFromDays(15),
                RequiresDuplicateDetection = true,
                EnableBatchedOperations = true,
                DuplicateDetectionHistoryTimeWindow = TimeSpanHelper.CreateTimeSpanFromDays(15),
                MaxDeliveryCount = 1000,
                //MaxSizeInMegabytes = FileSizeConverter.Convert(FileSizeProvider.GetFileSize(FileSize.Gb1),
                    //FileSizeUnit.Bytes, FileSizeUnit.Megabytes)
            };

            await _adminClient.CreateQueueAsync(options, cancellationToken);
            _logger.LogInformation($"Created queue {queueName}");
        }
    }


    /// <summary>
    /// Verifica si el nombre de la cola es válido.
    /// </summary>
    /// <param name="queueName">El nombre de la cola que se va a validar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el nombre de la cola es válido; de lo contrario, lanza una excepción <see cref="ArgumentException"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Se lanza cuando el nombre de la cola es nulo, vacío o solo contiene espacios en blanco.
    /// </exception>
    /// <remarks>
    /// Este método utiliza una expresión regular para validar el formato del nombre de la cola.
    /// Asegúrese de que el nombre de la cola cumpla con los requisitos establecidos por la expresión regular.
    /// </remarks>
    private bool IsValidQueueName(string queueName)
    {
        if (string.IsNullOrWhiteSpace(queueName))
            throw new ArgumentException("The queue name cannot be null or empty.");

        return RegularExpression.NameQueue.IsMatch(queueName);
    }
}