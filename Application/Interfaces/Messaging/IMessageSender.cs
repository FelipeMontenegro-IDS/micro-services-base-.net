namespace Domain.Messaging;

public interface IMessageSender
{
    Task SendMessageAsync<TRequest>( TRequest message, string queueOrTopicName);
}