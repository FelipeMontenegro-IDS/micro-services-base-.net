using Application.Interfaces;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repository;
using Shared.Services;

namespace Persistence;

public static class ServiceExtensions
{
    public static void AddPersistenceInfraestructure(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped<IDateTimeService, DateTimeService>();
        
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
            configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)   
        ));

        #region Repostiories
        
        services.AddScoped(typeof(IWriteRepositoryAsync<>), typeof(DbRepositoryAsync<>));
        services.AddScoped(typeof(IReadRepositoryAsync<>), typeof(DbRepositoryAsync<>));

        #endregion
    }
}