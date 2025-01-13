namespace Application.Interfaces.Azure.ServicesBus;

public interface IServiceBusQueueManager
{
    

    Task CreateQueueIfNotExists(string queueName, CancellationToken cancellationToken = default);
    bool IsValidQueueName(string queueName);

}