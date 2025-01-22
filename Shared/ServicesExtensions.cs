using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Bases.Lookup;
using Shared.Configurations;
using Shared.Converters;
using Shared.Helpers;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Data;
using Shared.Interfaces.Providers.Metadata;
using Shared.Interfaces.Providers.Queue.Messages.Requests;
using Shared.Interfaces.Providers.Queue.Messages.Responses;
using Shared.Interfaces.Providers.Queue.MicroServices;
using Shared.Interfaces.Providers.Queue.Services;
using Shared.Interfaces.Providers.Time;
using Shared.Interfaces.Wrappers;
using Shared.Providers.Data;
using Shared.Providers.Metadata;
using Shared.Providers.Queue.Messages.Requests;
using Shared.Providers.Queue.Messages.Responses;
using Shared.Providers.Queue.Services;
using Shared.Providers.Time;
using Shared.Wrappers.RetryPolicy;

namespace Shared;

public static class ServicesExtensions
{
    public static void AddApplicationShared(this IServiceCollection services, IConfiguration configuration)
    {
        #region Wrappers

        services.AddScoped(typeof(IRetryPolicy), typeof(RetryPolicy));

        #endregion

        #region Providers

        services.AddScoped(typeof(ILookup<,>), typeof(BaseLookupProvider<,>));
        services.AddScoped(typeof(IContentTypeProvider), typeof(ContentTypeProvider));
        services.AddScoped(typeof(IDateFormatProvider), typeof(DateFormatProvider));
        services.AddScoped(typeof(IFileSizeProvider), typeof(FileSizeProvider));
        services.AddScoped(typeof(ITimeSpanFormatProvider), typeof(TimeSpanFormatProvider));
        services.AddScoped(typeof(ITimeZoneProvider), typeof(TimeZoneProvider));
        services.AddScoped(typeof(ICacheControlProvider), typeof(CacheControlProvider));
        services.AddScoped(typeof(IContentDispositionProvider), typeof(ContentDispositionProvider));
        services.AddScoped(typeof(IContentEncodingProvider), typeof(ContentEncodingProvider));
        services.AddScoped(typeof(ISecretKeyProvider), typeof(SecretKeyProvider));
        services.AddScoped(typeof(IMicroServicesQueueRequestProvider),typeof(MicroServicesQueueRequestProvider));
        services.AddScoped(typeof(IMicroServicesQueueResponseProvider),typeof(MicroServicesQueueResponseProvider));
        services.AddScoped(typeof(IAuditQueueRequestProvider), typeof(AuditQueueRequestProvider));
        services.AddScoped(typeof(IConfigurationQueueRequestProvider), typeof(ConfigurationQueueRequestProvider));
        services.AddScoped(typeof(IPersonQueueRequestProvider), typeof(PersonQueueRequestProvider));
        services.AddScoped(typeof(IAuditQueueResponseProvider), typeof(AuditQueueResponseProvider));
        services.AddScoped(typeof(IConfigurationQueueResponseProvider), typeof(ConfigurationQueueResponseProvider));
        services.AddScoped(typeof(IPersonQueueResponseProvider), typeof(PersonQueueResponseProvider));
        services.AddScoped(typeof(IQueueResponseProvider), typeof(QueueResponseProvider));
        services.AddScoped(typeof(IQueueRequestProvider), typeof(QueueRequestProvider));
        
        #endregion

        #region Helpers

        services.AddScoped(typeof(IDateTimeHelper), typeof(DateTimeHelper));
        services.AddScoped(typeof(ITimeSpanHelper), typeof(TimeSpanHelper));
        services.AddScoped(typeof(IValidationHelper), typeof(ValidationHelper));
        services.AddScoped(typeof(IValueAssignmentHelper), typeof(ValueAssignmentHelper));
        services.AddScoped(typeof(IHashHelper), typeof(HashHelper));
        services.AddScoped(typeof(IPathHelper), typeof(PathHelper));
        services.AddScoped(typeof(IEncryptionHelper), typeof(EncryptionHelper));

        #endregion

        #region Converters

        services.AddScoped<FileSizeConverter>();
        services.AddScoped<TimeConverter>();

        #endregion

        #region Configurations

        services.Configure<EncryptionKey>(configuration.GetSection("EncryptionKey"));
        
        #endregion
    }
}