using Ardalis.Specification;
using Domain.Common;

namespace Application.Interfaces;

public interface IReadRepositoryAsync<T> : IReadRepositoryBase<T> where T : class
{
    
}