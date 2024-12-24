using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Domain.Messaging;

namespace Persistence.Messaging;

public class AzureServiceBusReceiver : IMessageReceiver
{
    private readonly ServiceBusClient _client;

    public AzureServiceBusReceiver(ServiceBusClient client)
    {
        _client = client;
    }
    public async Task RegisterMessageHandler<TResponse>(string queueOrTopicName, Func<TResponse, Task> processMessageAsync)
    {
        var processor = _client.CreateProcessor(queueOrTopicName);

        processor.ProcessMessageAsync += async args =>
        {
            var body = args.Message.Body.ToString();
            var message = JsonSerializer.Deserialize<TResponse>(body);

            if (message != null) await processMessageAsync(message);

            await args.CompleteMessageAsync(args.Message);
        };

        processor.ProcessErrorAsync += args =>
        {
            // Manejar errores aquí
            return Task.CompletedTask;
        };

        await processor.StartProcessingAsync();
    }
}