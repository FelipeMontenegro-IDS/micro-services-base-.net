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
    private readonly IServiceBusQueueTopicManager _queueTopicManager;

    public AzureServiceBusReceiver(
        ServiceBusClient client,
        ILoggerFactory loggerFactory,
        IValidationHelper validationHelper,
        IServiceBusQueueTopicManager queueTopicManager)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        _validationHelper = validationHelper ?? throw new ArgumentNullException(nameof(validationHelper));
        _queueTopicManager = queueTopicManager ?? throw new ArgumentNullException(nameof(queueTopicManager));
    }

    public async Task RegisterMessageHandler<T>(
        string queueOrTopicName,
        string? subscriptionName,
        Func<T, CancellationToken, Task> processMessageAsync,
        ServiceBusProcessorOptions options,
        CancellationToken cancellationToken = default) where T : class
    {
        bool queueExist = await _queueTopicManager.QueueExists(queueOrTopicName, cancellationToken);

        if (!queueExist)
        {
            bool isTopic = await _queueTopicManager.IsTopic(queueOrTopicName, cancellationToken);
            
            if (isTopic)
                await _queueTopicManager.CreateTopicIfNotExists(queueOrTopicName, cancellationToken);
            else
                await _queueTopicManager.CreateQueueIfNotExists(queueOrTopicName, cancellationToken);
        }
        else if (!string.IsNullOrEmpty(subscriptionName))
        {
            await _queueTopicManager.CreateSubscriptionIfNotExists(queueOrTopicName, subscriptionName,cancellationToken);
        }

        var processor = _validationHelper.IsNull(subscriptionName)
            ? _client.CreateProcessor(queueOrTopicName, options)
            : _client.CreateProcessor(queueOrTopicName, subscriptionName, options);

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