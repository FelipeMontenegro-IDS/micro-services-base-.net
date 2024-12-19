using Application.Interfaces;
using Ardalis.Specification.EntityFrameworkCore;
using Azure.Messaging.ServiceBus;
using Domain.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Messaging;
using Persistence.Repository;
using Shared.Configurations;
using Shared.Services;

namespace Persistence;

public static class ServiceExtensions
{
    public static void AddPersistenceInfraestructure(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped<IDateTimeService, DateTimeService>();

        #region DbContext and Azure Services Bus

        var serviceBusOptions = new AzureServiceBusOptions();
        configuration.GetSection("AzureServiceBus").Bind(serviceBusOptions);

        services.AddSingleton(serviceBusOptions);
        
        var client = new ServiceBusClient(serviceBusOptions.ConnectionString);
        services.AddSingleton(client);

        services.AddScoped(typeof(IMessageSender<>), typeof(AzureServiceBusSender<>));
        services.AddScoped(typeof(IMessageReceiver<>), typeof(AzureServiceBusReceiver<>));
        
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