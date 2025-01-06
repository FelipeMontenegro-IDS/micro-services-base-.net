using Ardalis.Specification;

namespace Application.Interfaces.Ardalis;

public interface IReadRepositoryAsync<T> : IReadRepositoryBase<T> where T : class
{
    
}