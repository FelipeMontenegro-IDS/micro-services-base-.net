using Shared.Bases.Lookup;
using Shared.Constants.Queue.Responses;
using Shared.Enums.Queue.Messages.responses;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Queue.Messages.Responses;

namespace Shared.Providers.Queue.Messages.Responses;

public class PersonQueueResponseProvider : BaseLookupProvider<PersonQueueResponse, string>, IPersonQueueResponseProvider
{
    public PersonQueueResponseProvider(IValidationHelper validationHelper) : base(
        new Dictionary<PersonQueueResponse, string>
        {
            { PersonQueueResponse.GetPersonByName, PersonQueueResponseConstant.QueueGetPersonByName },
            { PersonQueueResponse.GetPersonById, PersonQueueResponseConstant.QueueGetPersonById },
            { PersonQueueResponse.GetPersonByEmail, PersonQueueResponseConstant.QueueGetPersonByEmail },
            { PersonQueueResponse.PersonNotFound, PersonQueueResponseConstant.PersonNotFound }
        }, validationHelper)
    {
    }
}