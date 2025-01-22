using Shared.Constants.Queue.Responses;
using Shared.Enums.Queue.Messages.requests;
using Shared.Enums.Queue.Messages.responses;
using Shared.Interfaces.Providers.Queue.Messages.Responses;

namespace Shared.Providers.Queue.Messages.Responses;

public class QueueResponseProvider : IQueueResponseProvider
{
    private readonly Dictionary<Type, (Func<Enum, string>, string)> _queueProviders;

    public QueueResponseProvider(
        IAuditQueueResponseProvider auditQueueResponseProvider,
        IPersonQueueResponseProvider personQueueResponseProvider,
        IConfigurationQueueResponseProvider configurationQueueResponseProvider)
    {
        _queueProviders = new Dictionary<Type, (Func<Enum, string>, string)>
        {
            {
                typeof(AuditQueueResponse),
                (queue => auditQueueResponseProvider.GetValue((AuditQueueResponse)queue, AuditQueueResponseConstant.AuditNotFound),
                    AuditQueueResponseConstant.AuditNotFound)
            },
            {
                typeof(PersonQueueResponse),
                (queue => personQueueResponseProvider.GetValue((PersonQueueResponse)queue, PersonQueueResponseConstant.PersonNotFound),
                    PersonQueueResponseConstant.PersonNotFound)
            },
            {
                typeof(ConfigurationQueueResponse),
                (queue => configurationQueueResponseProvider.GetValue((ConfigurationQueueResponse)queue, ConfigurationQueueResponseConstant.ConfigurationNotFound),
                    ConfigurationQueueResponseConstant.ConfigurationNotFound)
            }
        };
    }

    public string GetQueueName(Type queueType, Enum queue)
    {
        if (_queueProviders.TryGetValue(queueType, out var provider))
        {
            return provider.Item1(queue);
        }

        throw new ArgumentException($"No matching queue provider found for the given queue type {queueType.Name}.");
    }
}