using Shared.Configurations;

namespace Application.Interfaces.Azure.ServicesBus;

public interface IMessageSender
{
    Task SendMessageAsync<TRequest>(
        TRequest message, string queue,
        CancellationToken cancellationToken = default,
        AzureProperties? properties = null
    );
}