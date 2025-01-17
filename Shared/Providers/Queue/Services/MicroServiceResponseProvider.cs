using Shared.Bases.Lookup;
using Shared.Enums.Queue.Services;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Queue.Services;

namespace Shared.Providers.Queue.Services;

public class MicroServiceResponseProvider : BaseLookupProvider<MicroService, QueueMessageResponse>,
    IMicroServiceResponseProvider
{
    public MicroServiceResponseProvider(IValidationHelper validationHelper)
        : base(
            new Dictionary<MicroService, QueueMessageResponse>
            {
                { MicroService.Audit, QueueMessageResponse.AuditMessage },
                { MicroService.Configuration, QueueMessageResponse.ConfigurationMessage },
                { MicroService.Person, QueueMessageResponse.PersonMessage }
            }, validationHelper)
    { }
}