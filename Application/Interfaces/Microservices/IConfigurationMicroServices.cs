using Application.Interfaces.Common;

namespace Application.Interfaces.Microservices;

public interface IConfigurationMicroServices
{
    public Task<ObjectTestResponse> GetConfigurationBlobStorageByCustomerId(ObjectTestRequest request,CancellationToken cancellationToken);
}