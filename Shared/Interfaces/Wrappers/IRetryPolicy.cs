using Shared.Enums;

namespace Shared.Interfaces.Wrappers;

public interface IRetryPolicy 
{
    Task RetryPolicyAsync(Func<Task> operation, RetryPolicyDefaults retryPolicyDefaults,
        CancellationToken cancellationToken = default);
}