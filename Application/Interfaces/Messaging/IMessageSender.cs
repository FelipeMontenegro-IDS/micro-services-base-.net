namespace Application.Interfaces.Messaging;

public interface IMessageSender
{
    Task SendMessageAsync<TRequest>( TRequest message, string queueOrTopicName,CancellationToken cancellationToken = default);
}