using Shared.Enums;

namespace Application.Interfaces.Azure.ServicesBus;

public interface IMessageRetryPolicy
{
    Task RetryPolicyAsync(Func<Task> operation, RetryPolicyDefaults retryPolicyDefaults,
        CancellationToken cancellationToken = default);
}