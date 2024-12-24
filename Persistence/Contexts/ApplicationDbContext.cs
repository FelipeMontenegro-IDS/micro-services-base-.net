using System.Reflection;
using Domain.Bases;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Utils.Generals;

namespace Persistence.Contexts;

public class ApplicationDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = DateTimeHelper.GetCurrentUtcDateTime();
                    entry.Entity.Modified = DateTimeHelper.GetCurrentUtcDateTime();
                    break;
                case EntityState.Modified:
                    entry.Entity.Modified = DateTimeHelper.GetCurrentUtcDateTime();
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