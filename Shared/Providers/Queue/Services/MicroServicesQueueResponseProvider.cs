using Shared.Bases.Lookup;
using Shared.Enums.Data;
using Shared.Enums.Queue.Messages.responses;
using Shared.Enums.Queue.Services;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Queue.MicroServices;

namespace Shared.Providers.Queue.Services;

public class MicroServicesQueueResponseProvider : BaseLookupProvider<MicroService, Type>,
    IMicroServicesQueueResponseProvider
{
    public MicroServicesQueueResponseProvider(IValidationHelper validationHelper)
        : base(new Dictionary<MicroService, Type>
        {
            { MicroService.Person, typeof(PersonQueueResponse) },
            { MicroService.Audit, typeof(AuditQueueResponse) },
            { MicroService.Configuration, typeof(ConfigurationQueueResponse) },
            { MicroService.NotFound, typeof(NotFound) }
        }, validationHelper)
    {
    }
}