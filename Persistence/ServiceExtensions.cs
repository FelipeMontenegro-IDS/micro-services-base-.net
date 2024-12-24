using Application.Interfaces;
using Azure.Messaging.ServiceBus;
using Domain.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Messaging;
using Persistence.Repositories;
using Shared.Configurations;

namespace Persistence;

public static class ServiceExtensions
{
    public static void AddPersistenceInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {
        #region DbContext and Azure Services Bus

        var serviceBusOptions = new AzureServiceBusOptions();

        IConfiguration configurationSection = configuration.GetSection("AzureServiceBus");
        serviceBusOptions.ConnectionString = configurationSection["ConnectionString"];

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

        services.AddScoped(typeof(IMessageSender), typeof(AzureServiceBusSender));
        services.AddScoped(typeof(IMessageReceiver), typeof(AzureServiceBusReceiver));
        services.AddScoped(typeof(IMessageService),typeof(AzureServiceBusMessage));


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