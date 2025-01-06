using Ardalis.Specification;

namespace Application.Interfaces.Ardalis;

public interface IWriteRepositoryAsync<T> : IRepositoryBase<T> where  T : class
{
    
}