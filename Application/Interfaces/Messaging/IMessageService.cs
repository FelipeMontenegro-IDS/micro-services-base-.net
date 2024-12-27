using Azure.Messaging.ServiceBus;

namespace Application.Interfaces.Messaging;

public interface IMessageService 
{
    Task<TResponse> ProcessRequestAsync<TRequest, TResponse>(
        TRequest request,
        string queueRequest,
        string queueResponse,
        CancellationToken cancellationToken = default) where TRequest : class where TResponse : class;

    Task<TResponse> ReceiveAndResponseAsync<TRequest, TResponse>(string queueRequest,
        string queueResponse,
        Func<TRequest, CancellationToken, Task<TResponse>> processResponseToSend,
        CancellationToken cancellationToken = default) where TRequest : class where TResponse : class;
}