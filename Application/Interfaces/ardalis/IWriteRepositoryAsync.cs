using Ardalis.Specification;

namespace Application.Interfaces;

public interface IWriteRepositoryAsync<T> : IRepositoryBase<T> where  T : class
{
    
}