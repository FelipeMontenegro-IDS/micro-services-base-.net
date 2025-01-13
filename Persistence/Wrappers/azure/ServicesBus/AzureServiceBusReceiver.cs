using Application.Interfaces.Azure.ServicesBus;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Shared.Helpers;
using Shared.Interfaces.Helpers;

namespace Persistence.Wrappers.azure.ServicesBus;

public class AzureServiceBusReceiver : IMessageReceiver
{
    private readonly ServiceBusClient _client;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IValidationHelper _validationHelper;

    public AzureServiceBusReceiver(
        ServiceBusClient client,
        ILoggerFactory loggerFactory,
        IValidationHelper validationHelper)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        _validationHelper = validationHelper ?? throw new ArgumentNullException(nameof(validationHelper));
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
            var consumeContext = new ProcessedMessageContext<T>(args, loggerForContext);
            var message = consumeContext.Message;

            if (_validationHelper.IsNotNull(message))
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

        processor.ProcessErrorAsync += (args) => Task.CompletedTask;

        await processor.StartProcessingAsync(cancellationToken);
    }
}