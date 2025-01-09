using System.Reflection;
using Application.Behaviours;
using Application.Wrappers.Azure.BlobStorage;
using Azure.Messaging.ServiceBus.Administration;
using Azure.Storage.Blobs;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        var connectionString = configuration.GetConnectionString("AzureBlobStorage");
        
        services.AddSingleton<AzureBlobStorage>(sp =>
        { // Get the connection string from configuration
            var blobClient = new BlobServiceClient(connectionString);
            return new AzureBlobStorage(blobClient ?? throw new InvalidOperationException(nameof(BlobServiceClient)));
        });

    }
}