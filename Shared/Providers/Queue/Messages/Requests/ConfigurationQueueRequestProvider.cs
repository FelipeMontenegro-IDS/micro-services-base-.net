using Shared.Bases.Lookup;
using Shared.Constants.Queue.Requests;
using Shared.Enums.Queue.Messages.requests;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Queue.Messages.Requests;

namespace Shared.Providers.Queue.Messages.Requests;

public class ConfigurationQueueRequestProvider : BaseLookupProvider<ConfigurationQueueRequest,string>,IConfigurationQueueRequestProvider
{
    public ConfigurationQueueRequestProvider(IValidationHelper validationHelper) 
        : base(new Dictionary<ConfigurationQueueRequest, string>
        {
            { ConfigurationQueueRequest.GetConfigurationByCustomerId, ConfigurationQueueRequestConstant.QueueGetConfigurationByCustomerId},
            { ConfigurationQueueRequest.ConfigurationNotFound, ConfigurationQueueRequestConstant.ConfigurationNotFound}
            
        }, validationHelper)
    {
    }
}