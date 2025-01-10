using Shared.Enums;
using Shared.Helpers;
using Shared.Interfaces.RetryPolicy;

namespace Shared.Wrappers.RetryPolicy;

public class RetryPolicy : IRetryPolicy
{
    public async Task RetryPolicyAsync(Func<Task> operation, RetryPolicyDefaults retryPolicyDefaults,
        CancellationToken cancellationToken = default)
    {
        if (ValidationHelper.IsNull(operation)) throw new ArgumentNullException(nameof(operation));

        (int maxRetryAttempts, TimeSpan delay) = retryPolicyDefaults switch
        {
            RetryPolicyDefaults.LowRetries => (2, TimeSpan.FromSeconds(1)),
            RetryPolicyDefaults.MediumRetries => (3, TimeSpan.FromSeconds(2)),
            RetryPolicyDefaults.HighRetries => (5, TimeSpan.FromSeconds(5)),
            RetryPolicyDefaults.VeryHighRetries => (7, TimeSpan.FromSeconds(10)),
            RetryPolicyDefaults.ExtremeRetries => (10, TimeSpan.FromSeconds(15)),
            RetryPolicyDefaults.NoRetries => (1, TimeSpan.Zero), // Solo un intento
            _ => throw new ArgumentOutOfRangeException(nameof(retryPolicyDefaults))
        };

        if (maxRetryAttempts <= 0) throw new ArgumentOutOfRangeException(nameof(maxRetryAttempts));
        if (delay < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(delay));


        int attempt = 0;
        do
        {
            try
            {
                attempt++;
                // Attempt the operation
                await operation();
                return; // If successful, exit the method
            }
            catch (Exception ex) when (attempt < maxRetryAttempts && !cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine($"Retry attempt {attempt} failed: {ex.Message}");
                await Task.Delay(delay, cancellationToken); // Wait before the next attempt
            }
        } while (attempt < maxRetryAttempts);

        throw new InvalidOperationException($"Operation failed after {maxRetryAttempts} attempts.");
    }
}