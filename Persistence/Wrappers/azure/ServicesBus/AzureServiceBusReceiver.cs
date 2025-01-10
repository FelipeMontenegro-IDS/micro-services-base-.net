using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces.Azure.ServicesBus;
using Shared.Helpers;

namespace Persistence.Wrappers.azure.ServicesBus;

public class AzureServiceBusReceiver : IMessageReceiver 
{
    private readonly ServiceBusClient _client;
    private readonly ILogger<AzureServiceBusReceiver> _receiverLogger;
    private readonly ILoggerFactory _loggerFactory;
    public AzureServiceBusReceiver(ServiceBusClient client, ILogger<AzureServiceBusReceiver> receiverLogger, ILoggerFactory loggerFactoryr)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _receiverLogger = receiverLogger ?? throw new ArgumentNullException(nameof(receiverLogger));
        _loggerFactory = loggerFactoryr ?? throw new ArgumentNullException(nameof(loggerFactoryr));
    }

    public async Task RegisterMessageHandler<T>(string queue,
        Func<T, CancellationToken, Task> processMessageAsync, 
        ServiceBusProcessorOptions options,
        CancellationToken cancellationToken = default) where T : class
    {
        var processor = _client.CreateProcessor(queue, options);
        
        processor.ProcessMessageAsync += async (args) =>
        {
            var messageId = args.Message.MessageId;
            var loggerForContext = _loggerFactory.CreateLogger<ProcessedMessageContext<T>>();
            var consumeContext = new ProcessedMessageContext<T>(args,loggerForContext);
            var message = consumeContext.Message;

            if (ValidationHelper.IsNotNull(message))
                try
                {
                    await processMessageAsync(message, cancellationToken);
                }
                catch (Exception ex)
                {
                    await args.AbandonMessageAsync(args.Message, null, cancellationToken);
                    return;
                }

            await args.CompleteMessageAsync(args.Message, cancellationToken);
        };

        processor.ProcessErrorAsync += (args) =>
        {
            // Manejar errores aquí
            return Task.CompletedTask;
        };

        await processor.StartProcessingAsync(cancellationToken);
    }
}