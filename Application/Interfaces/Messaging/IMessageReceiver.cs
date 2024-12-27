using Azure.Messaging.ServiceBus;

namespace Application.Interfaces.Messaging;

public interface IMessageReceiver
{
    Task RegisterMessageHandler<T>(string queueOrTopicName,
        Func<T, CancellationToken, Task> processMessageAsync, 
        ServiceBusProcessorOptions options,
        CancellationToken cancellationToken = default) where T : class;
}