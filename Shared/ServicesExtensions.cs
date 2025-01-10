using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Interfaces.RetryPolicy;
using Shared.Wrappers.RetryPolicy;

namespace Shared;

public static class ServicesExtensions
{
    public static void AddApplicationShared(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped(typeof(IRetryPolicy), typeof(RetryPolicy));

    }
}

