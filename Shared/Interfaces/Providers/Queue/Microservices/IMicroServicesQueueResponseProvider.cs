using Shared.Enums.Queue.Services;
using Shared.Interfaces.Lookup;

namespace Shared.Interfaces.Providers.Queue.MicroServices;

public interface IMicroServicesQueueResponseProvider : ILookupProvider<MicroService,Type>
{
    
}