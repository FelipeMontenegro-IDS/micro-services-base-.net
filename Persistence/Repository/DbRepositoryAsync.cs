using Application.Interfaces;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repository;

public class DbRepositoryAsync<T> : RepositoryBase<T>, IWriteRepositoryAsync<T>,
    IReadRepositoryAsync<T>
    where T : class
{
    private readonly ApplicationDbContext _dbContext;
    
    public DbRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext ?? 
                     throw new ArgumentNullException(nameof(dbContext), "The provided DbContext is null.");    
    }
}