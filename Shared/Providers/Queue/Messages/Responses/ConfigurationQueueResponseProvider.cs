using Shared.Bases.Lookup;
using Shared.Constants.Queue.Responses;
using Shared.Enums.Queue.Messages.responses;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Queue.Messages.Responses;

namespace Shared.Providers.Queue.Messages.Responses;

public class ConfigurationQueueResponseProvider : BaseLookupProvider<ConfigurationQueueResponse, string>,
    IConfigurationQueueResponseProvider
{
    public ConfigurationQueueResponseProvider(IValidationHelper validationHelper)
        : base(new Dictionary<ConfigurationQueueResponse, string>
        {
            {
                ConfigurationQueueResponse.GetConfigurationByCustomerId,
                ConfigurationQueueResponseConstant.QueueGetConfigurationByCustomerId
            },
            {
                ConfigurationQueueResponse.ConfigurationNotFound,
                ConfigurationQueueResponseConstant.ConfigurationNotFound
            }
        }, validationHelper)
    {
    }
}