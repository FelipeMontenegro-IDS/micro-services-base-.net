namespace Application.Interfaces.Microservices;

public interface IConfigurationMicroServices
{
    public Task<string> GetConfigurationBlobStorageByCustomerId(Guid customerId);
}