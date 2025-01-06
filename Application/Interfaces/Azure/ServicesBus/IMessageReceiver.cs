using Azure.Messaging.ServiceBus;

namespace Application.Interfaces.Azure.ServicesBus;

public interface IMessageReceiver
{
    Task RegisterMessageHandler<T>(
        string queueOrTopicName,
        Func<T, CancellationToken, Task> processMessageAsync,
        ServiceBusProcessorOptions options,
        CancellationToken cancellationToken = default
    ) where T : class;
}