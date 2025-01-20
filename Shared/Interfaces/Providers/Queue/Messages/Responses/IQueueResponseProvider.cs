namespace Shared.Interfaces.Providers.Queue.Messages.Responses;

public interface IQueueResponseProvider
{
    string GetQueueName(Type queueType, Enum queue);
}