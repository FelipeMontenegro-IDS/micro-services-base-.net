using System.Reflection;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

public class ApplicationDbContext : DbContext
{
    private readonly IDateTimeService _dateTime;
    public DbSet<Customer> Customers { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IDateTimeService dateTime) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        _dateTime = dateTime; 
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = _dateTime.nowUtc;
                    break;
                case EntityState.Modified:
                    entry.Entity.Modified = _dateTime.nowUtc;
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