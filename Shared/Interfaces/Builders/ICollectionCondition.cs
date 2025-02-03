using System.Linq.Expressions;

namespace Shared.Interfaces.Builders;

public interface ICollectionCondition<out TBuilder, T>
{
    TBuilder NotEmpty(Expression<Func<T, IEnumerable<object?>>> selector);

    TBuilder In<TValue>(Expression<Func<T, TValue>> selector, IEnumerable<TValue> values);

    TBuilder NotIn<TValue>(Expression<Func<T, TValue>> selector, IEnumerable<TValue> values);
}