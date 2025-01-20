using Shared.Bases.Lookup;
using Shared.Constants.Queue.Responses;
using Shared.Enums.Queue.Messages.responses;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Queue.Messages.Responses;

namespace Shared.Providers.Queue.Messages.Responses;

public class AuditQueueResponseProvider : BaseLookupProvider<AuditQueueResponse,string> ,IAuditQueueResponseProvider
{
    public AuditQueueResponseProvider(IValidationHelper validationHelper) 
        : base(new Dictionary<AuditQueueResponse, string>
        {
            { AuditQueueResponse.GetAuditContractById, AuditQueueResponseConstant.GetAuditContractById },
            { AuditQueueResponse.GetAuditContractByCustomerId, AuditQueueResponseConstant.GetAuditContractByCustomerId },
            { AuditQueueResponse.AuditNotFound, AuditQueueResponseConstant.AuditNotFound},
        }, validationHelper)
    {
    }
}