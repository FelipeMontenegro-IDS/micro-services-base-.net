using Shared.Configurations;

namespace Persistence.Interfaces.Azure.ServicesBus;

public interface IMessageSender
{
    Task SendMessageAsync<TRequest>(
        TRequest message, string queue,
        CancellationToken cancellationToken = default,
        AzureProperties? properties = null
    );
}