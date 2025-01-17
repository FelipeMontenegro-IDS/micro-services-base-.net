using Shared.Enums.Queue.Services;
using Shared.Interfaces.Lookup;

namespace Shared.Interfaces.Providers.Queue.Services;

public interface IMicroServiceRequestProvider : ILookupProvider<MicroService,QueueMessageRequest>
{
}