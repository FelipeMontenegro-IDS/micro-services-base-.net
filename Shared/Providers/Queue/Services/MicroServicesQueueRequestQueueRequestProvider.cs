using Shared.Bases.Lookup;
using Shared.Enums.Queue.Messages.requests;
using Shared.Enums.Queue.Services;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Queue.Services;

namespace Shared.Providers.Queue.Services;

public class MicroServicesQueueRequestQueueRequestProvider : BaseLookupProvider<MicroService, Type>,
    IMicroServicesQueueRequestProvider
{
    protected MicroServicesQueueRequestQueueRequestProvider(IValidationHelper validationHelper)
        : base(new Dictionary<MicroService, Type>
        {
            { MicroService.Person, typeof(PersonQueueRequest) },
            { MicroService.Audit, typeof(AuditQueueRequest) },
            { MicroService.Configuration, typeof(ConfigurationQueueRequest) }
        }, validationHelper)
    {}
}