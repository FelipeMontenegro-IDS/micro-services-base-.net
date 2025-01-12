using Application.Interfaces.Ardalis;
using Application.Interfaces.Microservices;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Interfaces.Azure.ServicesBus;
using Persistence.Microservices.Configurations;
using Persistence.Repositories;
using Persistence.Wrappers.azure.BlobStorage;
using Persistence.Wrappers.azure.ServicesBus;
using Shared.Configurations;

namespace Persistence;

public static class ServiceExtensions
{
    public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        #region DbContext and Azure Services Bus

        var serviceBusOptions = new AzureServiceBusOptions();

        IConfiguration configurationSection = configuration.GetSection("AzureServiceBus");
        serviceBusOptions.ConnectionString = configurationSection["ConnectionString"];

        services.AddSingleton(new ServiceBusAdministrationClient(configurationSection["ConnectionString"]));


        services.AddOptions<AzureServiceBusOptions>()
            .Bind(configuration.GetSection("AzureServiceBus"))
            .Validate(options => !string.IsNullOrEmpty(options.ConnectionString),
                "ConnectionString cannot be null or empty");

        if (!string.IsNullOrEmpty(serviceBusOptions.ConnectionString))
        {
            var client = new ServiceBusClient(serviceBusOptions.ConnectionString);
            services.AddSingleton(client);
        }
        else
        {
            throw new ArgumentException("AzureServiceBus connection string is missing");
        }

        services.AddSingleton<AzureBlobStorage>(sp =>
        { // Get the connection string from configuration
            var connectionString = configuration.GetConnectionString("AzureBlobStorage");
            var blobClient = new BlobServiceClient(connectionString);
            return new AzureBlobStorage(blobClient ?? throw new InvalidOperationException(nameof(BlobServiceClient)));
        });
        
        services.AddScoped(typeof(IMessageSender), typeof(AzureServiceBusSender));
        services.AddScoped(typeof(IMessageReceiver), typeof(AzureServiceBusReceiver));
        services.AddScoped(typeof(IConfigurationMicroServices), typeof(ConfigurationMicroServices));
        services.AddScoped(typeof(IMessage), typeof(AzureServiceBusMessage));
        services.AddScoped(typeof(IServiceBusQueueManager),typeof(ServiceBusQueueManager));

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
            configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
        ));

        #endregion

        #region Repostiories

        services.AddScoped(typeof(IWriteRepositoryAsync<>), typeof(DbRepositoryAsync<>));
        services.AddScoped(typeof(IReadRepositoryAsync<>), typeof(DbRepositoryAsync<>));

        #endregion
    }
}