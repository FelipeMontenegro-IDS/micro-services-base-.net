using System.Reflection;
using Application.Interfaces.Ardalis;
using Application.Interfaces.Azure.BlobStorage;
using Application.Interfaces.Azure.ServicesBus;
using Application.Interfaces.Microservices;
using Application.Interfaces.services;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Interfaces.EntityFramework;
using Persistence.Microservices.Configurations;
using Persistence.Repositories;
using Persistence.Services;
using Persistence.Wrappers.azure.BlobStorage;
using Persistence.Wrappers.azure.ServicesBus;
using Shared.Builders;
using Shared.Configurations;

namespace Persistence;

public static class ServiceExtensions
{
    public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        #region DbContext and Azure Services Bus

        services.AddHttpContextAccessor();
        
        var serviceBusOptions = new AzureServiceBusOption();

        IConfiguration configurationSection = configuration.GetSection("AzureServiceBus");
        serviceBusOptions.ConnectionString = configurationSection["ConnectionString"];

        services.AddSingleton(new ServiceBusAdministrationClient(configurationSection["ConnectionString"]));
        
        services.AddOptions<AzureServiceBusOption>()
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

        services.Configure<AzureBlobStorageOption>(configuration.GetSection("AzureBlobStorage"));
        services.AddSingleton<BlobServiceClient>(sp =>
        {
            var azureBlobConfig = new AzureBlobStorageOption();
            configuration.GetSection("AzureBlobStorage").Bind(azureBlobConfig);

            ConditionBuilder<AzureBlobStorageOption> cb = new ConditionBuilder<AzureBlobStorageOption>();
            cb.Add(option => string.IsNullOrWhiteSpace(option.AccountName))
                .Or()
                .Add(option => string.IsNullOrWhiteSpace(option.AccountKey))
                .Or()
                .Add(option => string.IsNullOrWhiteSpace(option.EndpointSuffix))
                .Or()
                .Add(option => string.IsNullOrWhiteSpace(option.Protocol));

            if (cb.Build(azureBlobConfig))
            {
                throw new ArgumentException("Los campos AccountName, AccountKey y EndpointSuffix son obligatorios.");
            }

            string connectionString = $"DefaultEndpointsProtocol={azureBlobConfig.Protocol};AccountName={azureBlobConfig.AccountName};AccountKey={azureBlobConfig.AccountKey};EndpointSuffix={azureBlobConfig.EndpointSuffix}";

            return new BlobServiceClient(connectionString ?? throw new InvalidOperationException(nameof(BlobServiceClient)));
        });

        services.AddScoped(typeof(IAzureBlobStorage), typeof(AzureBlobStorage));
        services.AddScoped(typeof(IMessageSender), typeof(AzureServiceBusSender));
        services.AddScoped(typeof(IMessageReceiver), typeof(AzureServiceBusReceiver));
        services.AddScoped(typeof(IConfigurationMicroServices), typeof(ConfigurationMicroServices));
        services.AddScoped(typeof(IMessage), typeof(AzureServiceBusMessage));
        services.AddScoped(typeof(IServiceBusQueueTopicManager), typeof(ServiceBusQueueTopicManager));
        services.AddScoped(typeof(IBaseQueue<>), typeof(ResponseQueue<>));
        services.AddScoped(typeof(IBaseQueue<>), typeof(RequestQueue<>));
        services.AddScoped(typeof(IRequestQueueFactory), typeof(RequestQueueFactory));
        services.AddScoped(typeof(IResponseQueueFactory), typeof(ResponseQueueFactory));
        services.AddScoped(typeof(ITimezoneService), typeof(TimezoneService));

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