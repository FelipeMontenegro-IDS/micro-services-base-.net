using System.Text.Json;
using Application.Interfaces.Messaging;
using Azure.Messaging.ServiceBus;
using Persistence.Wrappers;

namespace Persistence.Messaging;

public class AzureServiceBusReceiver : IMessageReceiver
{
    private readonly ServiceBusClient _client;

    public AzureServiceBusReceiver(ServiceBusClient client)
    {
        _client = client;
    }
    public async Task RegisterMessageHandler<T>(string queueOrTopicName, Func<T,CancellationToken ,Task> processMessageAsync,ServiceBusProcessorOptions options,CancellationToken cancellationToken = default) where T : class
    {
        var processor = _client.CreateProcessor(queueOrTopicName, options);

        processor.ProcessMessageAsync += async (args) =>
        {
            var consumeContext = new ConsumeContext<T>(args);
            var message = consumeContext.Message;

            if (message != null)
                try
                {
                    await processMessageAsync(message,cancellationToken);
                }
                catch (Exception ex)
                {
                    await args.AbandonMessageAsync(args.Message,null,cancellationToken); 
                    return;
                }

            await args.CompleteMessageAsync(args.Message,cancellationToken);
        };

        processor.ProcessErrorAsync += (args) =>
        {
            // Manejar errores aquí
            return Task.CompletedTask;
        };

        await processor.StartProcessingAsync(cancellationToken);
    }
}