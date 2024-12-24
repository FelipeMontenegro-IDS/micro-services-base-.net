using Application.Interfaces.Microservices;
using Domain.Messaging;

namespace Persistence.Microservices.Configurations;

public class ConfigurationMicroServices : IConfigurationMicroServices
{
    private readonly IMessageService _messageService;
    public ConfigurationMicroServices(IMessageService messageService)
    {
        _messageService = messageService;
    }
    
    public async Task<string> GetConfigurationBlobStorageByCustomerId(Guid customerId)
    {
        return await _messageService.SendRequestAsync<Guid, string>(customerId,"", "");
    }
    
}