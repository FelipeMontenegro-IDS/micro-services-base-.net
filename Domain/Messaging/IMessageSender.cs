namespace Domain.Messaging;

public interface IMessageSender<T>
{
    Task SendMessageAsync(T message, string queueOrTopicName);
}