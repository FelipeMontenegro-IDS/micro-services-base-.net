using System.Linq.Expressions;

namespace Shared.Interfaces.Builders;

public interface IComparisonCondition<out TBuilder, T>
{
    TBuilder NotNull(Expression<Func<T, object?>> selector);
    TBuilder Null(Expression<Func<T, object?>> selector);
    TBuilder EqualTo<TValue>(Expression<Func<T, TValue>> selector, TValue value) where TValue : IEquatable<TValue>;
    TBuilder NotEqualTo<TValue>(Expression<Func<T, TValue>> selector, TValue value) where TValue : IEquatable<TValue>;
}