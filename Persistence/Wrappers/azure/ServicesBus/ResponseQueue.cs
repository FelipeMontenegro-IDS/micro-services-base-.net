using Application.Interfaces.Azure.ServicesBus;
using Shared.Enums.Data;
using Shared.Enums.Queue.Services;
using Shared.Interfaces.Providers.Queue.Messages.Responses;
using Shared.Interfaces.Providers.Queue.MicroServices;

namespace Persistence.Wrappers.azure.ServicesBus;

public class ResponseQueue<TQueue> : IBaseQueue<TQueue> where TQueue : Enum
{
    private readonly IMicroServicesQueueResponseProvider _microServicesQueueResponseProvider;
    private readonly IQueueResponseProvider _queueResponseProvider;
    public MicroService Microservice { get; set; }
    public TQueue Queue { get; set; }

    public ResponseQueue(
        IMicroServicesQueueResponseProvider microServicesQueueResponseProvider,
        IQueueResponseProvider queueResponseProvider,
        MicroService microservice,
        TQueue queue)
    {
        _microServicesQueueResponseProvider = microServicesQueueResponseProvider;
        _queueResponseProvider = queueResponseProvider;
        Microservice = microservice;
        Queue = queue;

        if (IsValidQueue(microservice, queue))
            throw new ArgumentException(
                $" The microservice {nameof(microservice)} and the queue {nameof(queue)} do not match.");
    }


    public bool IsValidQueue(MicroService microservice, TQueue queue)
    {
        Type foundQueueType = _microServicesQueueResponseProvider.GetValue(microservice, typeof(NotFound));

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
        Type foundQueueType = _microServicesQueueResponseProvider.GetValue(microservice, typeof(NotFound));
        return _queueResponseProvider.GetQueueName(foundQueueType, queue);
    }
}