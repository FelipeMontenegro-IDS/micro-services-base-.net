using Shared.Enums.Queue.Services;

namespace Application.Interfaces.Azure.ServicesBus;

public interface IBaseQueue<TMicroservice, TQueue> where TMicroservice : Enum where TQueue : Enum
{
    public Enum Microservice { get; set; }
    public TQueue Queue { get; set; }
    bool IsValidQueue(MicroService microservice, Enum queue);
}