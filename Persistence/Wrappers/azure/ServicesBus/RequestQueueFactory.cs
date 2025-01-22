using Application.Interfaces.Azure.ServicesBus;
using Shared.Enums.Queue.Services;
using Shared.Interfaces.Providers.Queue.Messages.Requests;
using Shared.Interfaces.Providers.Queue.Services;

namespace Persistence.Wrappers.azure.ServicesBus;

public class RequestQueueFactory : IRequestQueueFactory
{
    private readonly IMicroServicesQueueRequestProvider _microServicesQueueRequestProvider;
    private readonly IQueueRequestProvider _queueRequestProvider;

    public RequestQueueFactory(IMicroServicesQueueRequestProvider microServicesQueueRequestProvider, IQueueRequestProvider queueRequestProvider)
    {
        _microServicesQueueRequestProvider = microServicesQueueRequestProvider;
        _queueRequestProvider = queueRequestProvider;
    }
    
    public IBaseQueue<TQueue> Create<TQueue>(MicroService microservice, TQueue queue) where TQueue : Enum
    {
        return new RequestQueue<TQueue>(
            _microServicesQueueRequestProvider, 
            _queueRequestProvider, 
            microservice,
            queue);
    }
}