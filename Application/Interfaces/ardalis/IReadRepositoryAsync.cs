using Ardalis.Specification;

namespace Application.Interfaces;

public interface IReadRepositoryAsync<T> : IReadRepositoryBase<T> where T : class
{
    
}