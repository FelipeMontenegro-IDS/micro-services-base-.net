using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;

namespace Persistence.Wrappers;

public class ProcessedMessageContext<T> where T : class
{
    public T? Message { get; }
    public string MessageId { get; }
    public ProcessMessageEventArgs ReceivedMessage { get; }

    private readonly ILogger<ProcessedMessageContext<T>> _logger;
    
    public ProcessedMessageContext(ProcessMessageEventArgs receivedMessage, ILogger<ProcessedMessageContext<T>> logger)
    {
        
        ReceivedMessage = receivedMessage ?? throw new ArgumentNullException(nameof(receivedMessage));
        MessageId = receivedMessage.Identifier;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        try
        {
            Message = JsonSerializer.Deserialize<T>(receivedMessage.Message.Body.ToString() ?? string.Empty);

            if (Message == null)
            {
                throw new JsonException($"Deserialized message is null: {MessageId}");
            }
            
        }
        catch (JsonException ex)
        {
            // Loguear el error, si es necesario
            Console.WriteLine($"Error deserializing message: {ex.Message}, content: {receivedMessage.Message.Body}");
            Message = null;
        }
        
    }
    
    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(MessageId))
        {
            _logger.LogError($"MessageId is null or empty, cannot complete message.");
            throw new InvalidOperationException("MessageId is required to complete the message.");
        }

        try
        {
            await ReceivedMessage.CompleteMessageAsync(ReceivedMessage.Message, cancellationToken);
            _logger.LogInformation($"Message {MessageId} successfully completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error completing message: {MessageId}");
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
            _logger.LogError($"Error abandoning message: {MessageId}, Exception: {ex.Message}");
            throw;
        }
        
    }

    public async Task DeadLetterAsync(string reason = "No specific reason", string errorDescription = "No additional info",CancellationToken cancellationToken = default)
    {
        try
        {
            await ReceivedMessage.DeadLetterMessageAsync(ReceivedMessage.Message, reason, errorDescription, cancellationToken);
            _logger.LogInformation($"Message {MessageId} moved to Dead-letter queue. Reason: {reason}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error dead-lettering message: {MessageId}, Reason: {reason}");
            throw;
        }
        
    }
}