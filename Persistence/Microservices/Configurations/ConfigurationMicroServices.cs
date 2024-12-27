using Application.Interfaces.common;
using Application.Interfaces.Messaging;
using Application.Interfaces.Microservices;
using Azure.Messaging.ServiceBus;
using Shared.Queues.Configurations;

namespace Persistence.Microservices.Configurations;

public class ConfigurationMicroServices : IConfigurationMicroServices
{
    private readonly IMessageService _messageService;

    public ConfigurationMicroServices(IMessageService messageService)
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