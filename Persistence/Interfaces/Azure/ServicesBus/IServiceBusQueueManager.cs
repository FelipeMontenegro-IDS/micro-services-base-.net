namespace Persistence.Interfaces.Azure.ServicesBus;

public interface IServiceBusQueueManager
{
    /// <summary>
    /// Crea una cola en Azure Service Bus si no existe, con opciones personalizadas.
    /// </summary>
    /// <param name="queueName">El nombre de la cola a crear.</param>
    /// <param name="cancellationToken">cancelar</param>
    /// <returns>Una tarea vacía que representa la operación asincrónica.</returns>
    Task CreateQueueIfNotExists(string queueName, CancellationToken cancellationToken = default);

}