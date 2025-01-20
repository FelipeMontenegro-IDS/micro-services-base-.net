using Shared.Constants.Queue.Requests;
using Shared.Constants.Queue.Responses;
using Shared.Enums.Queue.Messages.requests;
using Shared.Enums.Queue.Messages.responses;
using Shared.Interfaces.Providers.Queue.Messages.Requests;

namespace Shared.Providers.Queue.Messages.Requests;

public class QueueRequestProvider : IQueueRequestProvider
{
    private readonly Dictionary<Type, (Func<Enum, string>, string)> _queueProviders;


    public QueueRequestProvider(
        IAuditQueueRequestProvider auditQueueRequestProvider,
        IPersonQueueRequestProvider personQueueRequestProvider,
        IConfigurationQueueRequestProvider configurationQueueRequestProvider
    )
    {
        _queueProviders = new Dictionary<Type, (Func<Enum, string>, string)>
        {
            {
                typeof(AuditQueueResponse),
                (queue => auditQueueRequestProvider.GetValue((AuditQueueRequest)queue, AuditQueueRequestConstant.AuditNotFound),
                    AuditQueueRequestConstant.AuditNotFound)
            },
            {
                typeof(PersonQueueResponse),
                (queue => personQueueRequestProvider.GetValue((PersonQueueRequest)queue, PersonQueueRequestConstant.PersonNotFound),
                    PersonQueueRequestConstant.PersonNotFound)
            },
            {
                typeof(ConfigurationQueueResponse),
                (queue => configurationQueueRequestProvider.GetValue((ConfigurationQueueRequest)queue, ConfigurationQueueRequestConstant.ConfigurationNotFound),
                    ConfigurationQueueRequestConstant.ConfigurationNotFound)
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