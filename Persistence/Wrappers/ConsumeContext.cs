using System.Text.Json;
using Azure.Messaging.ServiceBus;

namespace Persistence.Wrappers;

public class ConsumeContext<T> where T : class
{
    public T Message { get; }
    public string MessageId { get; }
    public ProcessMessageEventArgs ReceivedMessage { get; }

    public ConsumeContext(ProcessMessageEventArgs receivedMessage)
    {
        
        ReceivedMessage = receivedMessage;
        MessageId = receivedMessage.Identifier;
        Message = JsonSerializer.Deserialize<T>(receivedMessage.Message.Body.ToString()) ?? throw new NullReferenceException("Message is null"); 
    }
    
    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        await ReceivedMessage.CompleteMessageAsync(ReceivedMessage.Message, cancellationToken); 
    }

    public async Task AbandonAsync(CancellationToken cancellationToken = default)
    {
        await ReceivedMessage.AbandonMessageAsync(ReceivedMessage.Message,null,cancellationToken);
    }

    public async Task DeadLetterAsync(CancellationToken cancellationToken = default)
    {
        await ReceivedMessage.DeadLetterMessageAsync(ReceivedMessage.Message,null,null,cancellationToken);
    }
}