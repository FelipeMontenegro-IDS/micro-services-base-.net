namespace Shared.Interfaces.Builders;

public interface IComparisonCondition<out TBuilder, T>
{
    TBuilder NotNull(Func<T, object?> selector);
    TBuilder Null(Func<T, object?> selector);
    TBuilder EqualTo<TValue>(Func<T, TValue> selector, TValue value) where TValue : IEquatable<TValue>;
    TBuilder NotEqualTo<TValue>(Func<T, TValue> selector, TValue value) where TValue : IEquatable<TValue>;
}