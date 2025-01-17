using Shared.Enums.Data;

namespace Shared.Interfaces.Wrappers;

public interface IRetryPolicy 
{
    Task RetryPolicyAsync(Func<Task> operation, RetryPolicyDefault retryPolicyDefault,
        CancellationToken cancellationToken = default);
}