using System.Reflection;
using Domain.Bases;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces.Helpers;

namespace Persistence.Contexts;

public class ApplicationDbContext : DbContext
{

    private readonly IDateTimeHelper _dateTimeHelper;
    
    public DbSet<Customer> Customers { get; set; }
    
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options, 
        IDateTimeHelper dateTimeHelper) 
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        _dateTimeHelper = dateTimeHelper;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created =  _dateTimeHelper.GetCurrentUtcDateTime();
                    entry.Entity.Modified = _dateTimeHelper.GetCurrentUtcDateTime();
                    break;
                case EntityState.Modified:
                    entry.Entity.Modified = _dateTimeHelper.GetCurrentUtcDateTime();
                    break;
                case EntityState.Deleted:
                    entry.Entity.Deleted = null;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}