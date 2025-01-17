using Application.Interfaces.Azure.ServicesBus;
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
public class ServiceBusQueueManager : IServiceBusQueueManager
{
    private readonly ServiceBusAdministrationClient _adminClient;
    private readonly ILogger<ServiceBusQueueManager> _logger;
    private readonly ITimeSpanHelper _timeSpanHelper;
    private readonly IFileSizeProvider _fileSizeProvider;
    private readonly FileSizeConverter _fileSizeConverter;
    
    public ServiceBusQueueManager(
        ServiceBusAdministrationClient adminClient,
        ILogger<ServiceBusQueueManager> logger,
        ITimeSpanHelper timeSpanHelper,
        IFileSizeProvider fileSizeProvider,
        FileSizeConverter fileSizeConverter)
    {
        _adminClient = adminClient ?? throw new ArgumentNullException(nameof(adminClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _timeSpanHelper = timeSpanHelper ?? throw new ArgumentNullException(nameof(timeSpanHelper));
        _fileSizeProvider = fileSizeProvider ?? throw new ArgumentNullException(nameof(fileSizeProvider));
        _fileSizeConverter = fileSizeConverter ?? throw new ArgumentNullException(nameof(fileSizeConverter));
    }

    /// <summary>
    /// Crea una cola en Azure Service Bus si no existe, con opciones personalizadas.
    /// </summary>
    /// <param name="queueName">El nombre de la cola a crear.</param>
    /// <param name="cancellationToken">cancelar</param>
    /// <returns>Una tarea vacía que representa la operación asincrónica.</returns>
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
    public bool IsValidQueueName(string queueName)
    {
        if (string.IsNullOrWhiteSpace(queueName))
            throw new ArgumentException("The queue name cannot be null or empty.");

        return RegularExpression.NameQueue.IsMatch(queueName);
    }
}