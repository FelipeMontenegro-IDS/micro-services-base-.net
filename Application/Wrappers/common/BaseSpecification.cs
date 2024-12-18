using System.Linq.Expressions;
using Ardalis.Specification;

namespace Application.Wrappers.Common.Responses;

public abstract class BaseSpecification<T> : Specification<T> where T : class
{
    protected BaseSpecification(
        Expression<Func<T, bool>>? criteria = null,
        Expression<Func<T, object?>>? orderBy = null,
        Expression<Func<T, object?>>? orderByDescending = null,
        List<Expression<Func<T, object>>>? includes = null,
        int? skip = null,
        int? take = null)
    {
        // Aplicar criterios
        if (criteria != null) Query.Where(criteria);

        // Ordenamientos
        if (orderBy != null) Query.OrderBy(orderBy);
        if (orderByDescending != null) Query.OrderByDescending(orderByDescending);

        // Relaciones (Includes)
        if (includes != null)
        {
            foreach (var include in includes)
                Query.Include(include);
        }

        // Paginación
        if (skip.HasValue) Query.Skip(skip.Value);
        if (take.HasValue) Query.Take(take.Value);
    }

    // Métodos adicionales para extender comportamiento
    public void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Query.Include(includeExpression);
    }

    public void AddOrderBy(Expression<Func<T, object?>> orderByExpression)
    {
        Query.OrderBy(orderByExpression);
    }

    public void AddOrderByDescending(Expression<Func<T, object?>> orderByDescendingExpression)
    {
        Query.OrderByDescending(orderByDescendingExpression);
    }

    public void ApplyPagination(int skip, int take)
    {
        Query.Skip(skip).Take(take);
    }
}