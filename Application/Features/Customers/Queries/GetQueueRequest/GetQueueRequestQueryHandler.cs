using Application.Interfaces.Azure.ServicesBus;
using MediatR;
using Shared.Enums.Queue.Messages.requests;
using Shared.Enums.Queue.Services;

namespace Application.Features.Customers.Queries.GetQueueRequest;

public class GetQueueRequestQueryHandler : IRequestHandler<GetQueueRequestQuery,string>
{
    private readonly IRequestQueueFactory _queueRequestFactory;

    public GetQueueRequestQueryHandler(IRequestQueueFactory queueRequestFactory)
    {
        _queueRequestFactory = queueRequestFactory;
    }
    
    public Task<string> Handle(GetQueueRequestQuery request, CancellationToken cancellationToken)
    {
        string queue = _queueRequestFactory.Create(MicroService.Person, PersonQueueRequest.GetPersonByName).GetQueueName();
        return Task.FromResult(queue);
    }
}