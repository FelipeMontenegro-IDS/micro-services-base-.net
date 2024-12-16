using System.Linq.Expressions;
using Ardalis.Specification;

namespace Application.Wrappers.common.responses;

public class BaseSpecification<T> : Specification<T> where T : class
{
    public BaseSpecification(Expression<Func<T, bool>>? expression = null,
        Expression<Func<T, object?>> orderBy = null!,
        List<Expression<Func<T, object>>>? includes = null)
    {
        if (expression != null) Query.Where(expression, false);
        ApplyOrderBy(orderBy);
        ApplyIncludes(includes);
    }
    
    private void ApplyOrderBy(Expression<Func<T, object?>> orderBy)
    {
        if (orderBy != null)
        {
            Query.OrderBy(orderBy);
        }
    }

    private void ApplyIncludes(List<Expression<Func<T, object>>>? includes)
    {
        if (includes != null && includes.Any())
        {
            foreach (var include in includes)
            {
                Query.Include(include,false);
            }
        }
    }
}