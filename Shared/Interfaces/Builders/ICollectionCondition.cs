using System.Linq.Expressions;

namespace Shared.Interfaces.Builders;

public interface ICollectionCondition<out TBuilder, T>
{
    TBuilder NotEmpty(Expression<Func<T, IEnumerable<object?>>> selector);
    TBuilder In<TValue>(Expression<Func<T, TValue>> selector, IEnumerable<TValue> values);
    TBuilder NotIn<TValue>(Expression<Func<T, TValue>> selector, IEnumerable<TValue> values);
    TBuilder Count(Expression<Func<T, IEnumerable<object?>>> selector, int count);
    TBuilder CountBetween(Expression<Func<T, IEnumerable<object?>>> selector, int min, int max);
    TBuilder All<TValue>(Expression<Func<T, IEnumerable<TValue>>> selector, Expression<Func<TValue, bool>> predicate);
    TBuilder Any<TValue>(Expression<Func<T, IEnumerable<TValue>>> selector, Expression<Func<TValue, bool>> predicate);
    TBuilder Contains<TValue>(Expression<Func<T, IEnumerable<TValue>>> selector, TValue value);
    TBuilder DistinctCount(Expression<Func<T, IEnumerable<object?>>> selector, int count);
    TBuilder None<TValue>(Expression<Func<T, IEnumerable<TValue>>> selector, Expression<Func<TValue, bool>> predicate);


}