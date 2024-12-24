namespace Domain.Messaging;

public interface IMessageService
{
    Task<TResponse> SendRequestAsync<TRequest, TResponse>(
        TRequest request, 
        string queueRequest, 
        string queueResponse);
}