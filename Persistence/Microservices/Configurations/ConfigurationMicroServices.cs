using Application.Interfaces.Azure.ServicesBus;
using Application.Interfaces.Common;
using Application.Interfaces.Microservices;
using Azure.Messaging.ServiceBus;
using Shared.Queues.Configurations;

namespace Persistence.Microservices.Configurations;

public class ConfigurationMicroServices : IConfigurationMicroServices
{
    private readonly IMessage  _messageService;

    public ConfigurationMicroServices(IMessage messageService)
    {
        _messageService = messageService;
    }

    public async Task<ObjectTestResponse> GetConfigurationBlobStorageByCustomerId(ObjectTestRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _messageService.ProcessRequestAsync<ObjectTestRequest, ObjectTestResponse>(
            request,
            QueueRequestConstants.REQ_CONFIGURATION_BLOB_STORAGE,
            QueueResponseConstants.RES_CONFIGURATION_BLOB_STORAGE,
            new ServiceBusProcessorOptions { AutoCompleteMessages = false },
            cancellationToken);

        return response;
    }
}