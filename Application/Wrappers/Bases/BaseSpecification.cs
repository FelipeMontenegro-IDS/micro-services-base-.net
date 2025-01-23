using System.Linq.Expressions;
using Application.Interfaces.Ardalis;
using Ardalis.Specification;

namespace Application.Wrappers.Bases;

public abstract class BaseSpecification<T> : Specification<T>, ISpecificationMethods<T> where T : class
{
    protected BaseSpecification(
        Expression<Func<T, bool>>? criteria = null,
        Expression<Func<T, object?>>? orderBy = null,
        Expression<Func<T, object?>>? orderByDescending = null,
        IEnumerable<Expression<Func<T, object>>>? includes = null,
        IEnumerable<(Expression<Func<T, string>> selector, string searchTerm, int searchGroup)>? searches = null,
        int? skip = null,
        int? take = null)
    {
        // Aplicar criterios
        if (criteria != null) Query.Where(criteria);

        // Aplicar criterios de búsqueda
        if (searches != null)
        {
            foreach (var search in searches)
            {
                if (!string.IsNullOrEmpty(search.searchTerm))
                    Query.Search(search.selector, $"%{search.searchTerm}%", search.searchGroup);
            }
        }

        // Ordenamientos
        if (orderBy != null) Query.OrderBy(orderBy);
        if (orderByDescending != null) Query.OrderByDescending(orderByDescending);

        // Relaciones (Includes)
        if (includes != null)
        {
            foreach (Expression<Func<T, object>> include in includes)
            {
                Query.Include(include);
            }
        }

        // Paginación
        if (skip.HasValue) Query.Skip(skip.Value);
        if (take.HasValue) Query.Take(take.Value);
    }

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

    public void AddSearch(Expression<Func<T, string>> selector, string searchTerm, int searchGroup)
    {
        if (!string.IsNullOrEmpty(searchTerm)) Query.Search(selector, $"%{searchTerm}%", searchGroup);
    }
}