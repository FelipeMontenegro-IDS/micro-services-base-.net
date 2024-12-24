namespace Domain.Messaging;

public interface IMessageReceiver
{
    Task RegisterMessageHandler<TResponse>(string queueOrTopicName, Func<TResponse, Task> processMessageAsync);
}