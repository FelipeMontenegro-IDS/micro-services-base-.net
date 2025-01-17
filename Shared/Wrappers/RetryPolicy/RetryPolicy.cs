using Shared.Enums.Data;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Wrappers;

namespace Shared.Wrappers.RetryPolicy;

public class RetryPolicy : IRetryPolicy
{
    private readonly IValidationHelper _validationHelper;

    public RetryPolicy(IValidationHelper validationHelper)
    {
        _validationHelper = validationHelper;
    }
    
    public async Task RetryPolicyAsync(Func<Task> operation, RetryPolicyDefault retryPolicyDefault,
        CancellationToken cancellationToken = default)
    {
        if (_validationHelper.IsNull(operation)) throw new ArgumentNullException(nameof(operation));

        (int maxRetryAttempts, TimeSpan delay) = retryPolicyDefault switch
        {
            RetryPolicyDefault.LowRetries => (2, TimeSpan.FromSeconds(1)),
            RetryPolicyDefault.MediumRetries => (3, TimeSpan.FromSeconds(2)),
            RetryPolicyDefault.HighRetries => (5, TimeSpan.FromSeconds(5)),
            RetryPolicyDefault.VeryHighRetries => (7, TimeSpan.FromSeconds(10)),
            RetryPolicyDefault.ExtremeRetries => (10, TimeSpan.FromSeconds(15)),
            RetryPolicyDefault.NoRetries => (1, TimeSpan.Zero), // Solo un intento
            _ => throw new ArgumentOutOfRangeException(nameof(retryPolicyDefault))
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