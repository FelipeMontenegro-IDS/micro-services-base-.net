using Application.Interfaces.Azure.ServicesBus;
using Shared.Enums.Data;
using Shared.Enums.Queue.Services;
using Shared.Interfaces.Providers.Queue.Messages.Requests;
using Shared.Interfaces.Providers.Queue.Services;

namespace Persistence.Wrappers.azure.ServicesBus;

public class RequestQueue<TQueue> : IBaseQueue<TQueue> where TQueue : Enum
{
    private readonly IMicroServicesQueueRequestProvider _microServicesQueueRequestProvider;
    private readonly IQueueRequestProvider _queueRequestProvider;
    public MicroService Microservice { get; set; }
    public TQueue Queue { get; set; }

    public RequestQueue(
        IMicroServicesQueueRequestProvider microServicesQueueRequestProvider,
        IQueueRequestProvider queueRequestProvider,
        MicroService microservice,
        TQueue queue)
    {
        _microServicesQueueRequestProvider = microServicesQueueRequestProvider;
        _queueRequestProvider = queueRequestProvider;
        Microservice = microservice;
        Queue = queue;

        if (IsValidQueue(microservice, queue))
            throw new ArgumentException($" The microservice {nameof(microservice)} and the queue {nameof(queue)} do not match.");
    }


    public bool IsValidQueue(MicroService microservice, TQueue queue)
    {
        Type foundQueueType = _microServicesQueueRequestProvider.GetValue(microservice, typeof(NotFound));

        if (foundQueueType != typeof(NotFound))
        {
            if (Enum.IsDefined(foundQueueType, queue))
            {
                return true;
            }
        }

        return false;
    }

    public string GetQueueName(MicroService microservice, TQueue queue)
    {
        Type foundQueueType = _microServicesQueueRequestProvider.GetValue(microservice, typeof(NotFound));
        return _queueRequestProvider.GetQueueName(foundQueueType, queue);
    }
}