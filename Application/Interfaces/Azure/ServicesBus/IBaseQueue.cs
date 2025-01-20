using Shared.Enums.Queue.Services;

namespace Application.Interfaces.Azure.ServicesBus;

public interface IBaseQueue<TQueue> where TQueue : Enum
{
    public MicroService Microservice { get; set; }
    public TQueue Queue { get; set; }
    bool IsValidQueue(MicroService microservice, TQueue queue);
    
    string GetQueueName(MicroService microservice, TQueue queue);
}