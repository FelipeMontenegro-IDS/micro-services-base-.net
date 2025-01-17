using Shared.Bases.Lookup;
using Shared.Enums.Queue.Services;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Queue.Services;

namespace Shared.Providers.Queue.Services;

public class MicroServiceRequestProvider : BaseLookupProvider<MicroService, QueueMessageRequest>,
    IMicroServiceRequestProvider
{
    public MicroServiceRequestProvider(IValidationHelper validationHelper)
        : base(
            new Dictionary<MicroService, QueueMessageRequest>
            {
                { MicroService.Audit, QueueMessageRequest.AuditMessage },
                { MicroService.Configuration, QueueMessageRequest.ConfigurationMessage },
                { MicroService.Person, QueueMessageRequest.PersonMessage },
            }, validationHelper)
    { }
}