using Shared.Bases.Lookup;
using Shared.Constants.Queue.Requests;
using Shared.Enums.Queue.Messages.requests;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Queue.Messages.Requests;

namespace Shared.Providers.Queue.Messages.Requests;

public class PersonQueueRequestProvider : BaseLookupProvider<PersonQueueRequest,string>, IPersonQueueRequestProvider
{
    public PersonQueueRequestProvider(IValidationHelper validationHelper) 
        : base(new Dictionary<PersonQueueRequest, string>
        {
            { PersonQueueRequest.GetPersonById, PersonQueueRequestConstant.QueueGetPersonById },
            { PersonQueueRequest.GetPersonByEmail, PersonQueueRequestConstant.QueueGetPersonByEmail },
            { PersonQueueRequest.GetPersonByName, PersonQueueRequestConstant.QueueGetPersonByName },
            { PersonQueueRequest.PersonNotFound, PersonQueueRequestConstant.PersonNotFound }
        }, validationHelper)
    {
    }
}