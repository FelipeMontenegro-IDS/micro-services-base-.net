using Application.Interfaces.Azure.ServicesBus;
using Shared.Enums.Queue.Services;
using Shared.Interfaces.Providers.Queue.Messages.Responses;
using Shared.Interfaces.Providers.Queue.MicroServices;

namespace Persistence.Wrappers.azure.ServicesBus;

public class ResponseQueueFactory : IResponseQueueFactory
{
    private readonly IMicroServicesQueueResponseProvider _microServicesQueueResponseProvider;
    private readonly IQueueResponseProvider _queueResponseProvider;

    public ResponseQueueFactory(
        IMicroServicesQueueResponseProvider microServicesQueueResponseProvider,
        IQueueResponseProvider queueResponseProvider)
    {
        _microServicesQueueResponseProvider = microServicesQueueResponseProvider;
        _queueResponseProvider = queueResponseProvider;
    }

    public IBaseQueue<TQueue> Create<TQueue>(MicroService microservice, TQueue queue) where TQueue : Enum
    {
        return new ResponseQueue<TQueue>(
            _microServicesQueueResponseProvider, 
            _queueResponseProvider, 
            microservice,
            queue);
    }
}