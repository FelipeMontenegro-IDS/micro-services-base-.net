using Shared.Bases.Lookup;
using Shared.Constants.Queue.Requests;
using Shared.Enums.Queue.Messages.requests;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Queue.Messages.Requests;

namespace Shared.Providers.Queue.Messages.Requests;

public class AuditQueueRequestProvider : BaseLookupProvider<AuditQueueRequest, string>, IAuditQueueRequestProvider
{
    public AuditQueueRequestProvider(IValidationHelper validationHelper)
        : base(new Dictionary<AuditQueueRequest, string>
        {
            { AuditQueueRequest.GetAuditContractByCustomerId, AuditQueueRequestConstant.GetAuditContractByCustomerId },
            { AuditQueueRequest.GetAuditContractById, AuditQueueRequestConstant.GetAuditContractById },
            { AuditQueueRequest.AuditNotFound, AuditQueueRequestConstant.AuditNotFound }
        }, validationHelper)
    {
    }
}