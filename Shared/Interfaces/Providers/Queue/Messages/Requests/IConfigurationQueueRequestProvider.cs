using Shared.Enums.Queue.Messages.requests;
using Shared.Interfaces.Lookup;

namespace Shared.Interfaces.Providers.Queue.Messages.Requests;

public interface IConfigurationQueueRequestProvider : ILookupProvider<ConfigurationQueueRequest,string>
{
    
}