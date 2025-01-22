using Shared.Enums.Queue.Services;

namespace Application.Interfaces.Azure.ServicesBus;

public interface IQueueFactory
{
    public IBaseQueue<TQueue> Create<TQueue>(MicroService microservice, TQueue queue) where TQueue : Enum;

}