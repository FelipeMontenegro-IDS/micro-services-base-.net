using Shared.Utils.Enums;

namespace Application.Interfaces.Messaging;

public interface IMessageRetryPolicy
{
    Task RetryPolicyAsync(Func<Task> operation, RetryPolicyDefaults retryPolicyDefaults,
        CancellationToken cancellationToken = default);
}