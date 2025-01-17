using Azure.Messaging.ServiceBus;
using Shared.Enums.Data;

namespace Application.Interfaces.Azure.ServicesBus;

public interface IMessage
{ 
    Task<TResponse> ProcessRequestAsync<TRequest, TResponse>(
        TRequest request,
        string queueRequest,
        string queueResponse,
        ServiceBusProcessorOptions options,
        CancellationToken cancellationToken = default,
        IDictionary<string, object>? headers = null,
        RetryPolicyDefault retryPolicyDefault = RetryPolicyDefault.NoRetries) where TRequest : class where TResponse : class;

    Task ReceiveAndResponseAsync<TRequest, TResponse>(
        string queueRequest,
        string queueResponse,
        ServiceBusProcessorOptions options,
        Func<TRequest, CancellationToken, Task<TResponse>> processResponseToSend,
        CancellationToken cancellationToken = default,
        IDictionary<string, object>? headers = null,
        RetryPolicyDefault retryPolicyDefault = RetryPolicyDefault.NoRetries) where TRequest : class where TResponse : class;
    
    Task SendAsync<T>(
        T message,
        string queue,
        IDictionary<string, object>? headers = null,
        CancellationToken cancellationToken = default,
        RetryPolicyDefault retryPolicyDefault = RetryPolicyDefault.NoRetries) where T : class;

    public Task<T> ProcessAndReturnMessageAsync<T>(
        string queue,
        Func<T, CancellationToken, Task<T>> processMessage,
        ServiceBusProcessorOptions options,
        CancellationToken cancellationToken = default)
        where T : class;
}