using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Bases.Lookup;
using Shared.Configurations;
using Shared.Converters;
using Shared.Helpers;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Data;
using Shared.Interfaces.Providers.Metadata;
using Shared.Interfaces.Providers.Time;
using Shared.Interfaces.Wrappers;
using Shared.Providers.Data;
using Shared.Providers.Metadata;
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