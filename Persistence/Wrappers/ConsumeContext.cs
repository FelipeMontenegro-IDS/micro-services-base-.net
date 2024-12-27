using System.Text.Json;
using Azure.Messaging.ServiceBus;

namespace Persistence.Wrappers;

public class ConsumeContext<T> where T : class
{
    public T? Message { get; }
    public string MessageId { get; }
    public ProcessMessageEventArgs ReceivedMessage { get; }

    public ConsumeContext(ProcessMessageEventArgs receivedMessage)
    {
        
        ReceivedMessage = receivedMessage ?? throw new ArgumentNullException(nameof(receivedMessage));
        MessageId = receivedMessage.Identifier;
        try
        {
            Message = JsonSerializer.Deserialize<T>(receivedMessage.Message.Body.ToString());
        }
        catch (JsonException ex)
        {
            // Loguear el error, si es necesario
            Console.WriteLine($"Error deserializing message: {ex.Message}");
            Message = null;
        }
        
    }
    
    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await ReceivedMessage.CompleteMessageAsync(ReceivedMessage.Message, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error completing message: {MessageId}, Exception: {ex.Message}");
            throw;
        }
        
    }

    public async Task AbandonAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await ReceivedMessage.AbandonMessageAsync(ReceivedMessage.Message, null, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error abandoning message: {MessageId}, Exception: {ex.Message}");
            throw;
        }
        
    }

    public async Task DeadLetterAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await ReceivedMessage.DeadLetterMessageAsync(ReceivedMessage.Message, null, null, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error dead-lettering message: {MessageId}, Exception: {ex.Message}");
            throw;
        }
        
    }
}