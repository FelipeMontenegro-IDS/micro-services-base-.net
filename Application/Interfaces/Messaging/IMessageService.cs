using Azure.Messaging.ServiceBus;
using Shared.Utils.Enums;

namespace Application.Interfaces.Messaging;

public interface IMessageService
{
    Task<TResponse> ProcessRequestAsync<TRequest, TResponse>(
        TRequest request,
        string queueRequest,
        string queueResponse,
        ServiceBusProcessorOptions options,
        CancellationToken cancellationToken = default,
        IDictionary<string, object>? headers = null,
        RetryPolicyDefaults retryPolicyDefaults = RetryPolicyDefaults.NoRetries) where TRequest : class where TResponse : class;

    Task ReceiveAndResponseAsync<TRequest, TResponse>(
        string queueRequest,
        string queueResponse,
        ServiceBusProcessorOptions options,
        Func<TRequest, CancellationToken, Task<TResponse>> processResponseToSend,
        CancellationToken cancellationToken = default,
        IDictionary<string, object>? headers = null,
        RetryPolicyDefaults retryPolicyDefaults = RetryPolicyDefaults.NoRetries) where TRequest : class where TResponse : class;
    
    Task SendAsync<T>(
        T message,
        string queue,
        IDictionary<string, object>? headers = null,
        CancellationToken cancellationToken = default,
        RetryPolicyDefaults retryPolicyDefaults = RetryPolicyDefaults.NoRetries) where T : class;

    public Task<T> ProcessAndReturnMessageAsync<T>(
        string queue,
        Func<T, CancellationToken, Task<T>> processMessage,
        ServiceBusProcessorOptions options,
        CancellationToken cancellationToken = default)
        where T : class;
}