namespace Shared.Interfaces.Providers.Queue.Messages.Requests;

public interface IQueueRequestProvider
{
    string GetQueueName(Type queueType, Enum queue);
}