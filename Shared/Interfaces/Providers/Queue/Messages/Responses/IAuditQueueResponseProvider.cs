using Shared.Enums.Queue.Messages.responses;
using Shared.Interfaces.Lookup;

namespace Shared.Interfaces.Providers.Queue.Messages.Responses;

public interface IAuditQueueResponseProvider : ILookupProvider<AuditQueueResponse,string>
{
    
}