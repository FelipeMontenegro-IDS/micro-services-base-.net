using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Utils.Helpers;

namespace Shared;

public static class ServicesExtensions
{
    public static void AddApplicationShared(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddSingleton<AzureBlobStorageHelper>(sp =>
        {
            // Get the connection string from configuration
            var connectionString = configuration.GetConnectionString("AzureBlobStorage");
            return new AzureBlobStorageHelper(connectionString ?? throw new InvalidOperationException());
        });

    }
}

