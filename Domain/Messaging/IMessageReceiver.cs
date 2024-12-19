namespace Domain.Messaging;

public interface IMessageReceiver<T>
{
    Task RegisterMessageHandler(string queueOrTopicName, Func<T, Task> processMessageAsync);
}