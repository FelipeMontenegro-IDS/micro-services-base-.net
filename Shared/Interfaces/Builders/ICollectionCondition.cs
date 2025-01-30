namespace Shared.Interfaces.Builders;

public interface ICollectionCondition<out TBuilder, T>
{
    TBuilder NotEmpty(Func<T, IEnumerable<object?>> selector);

    TBuilder In<TValue>(Func<T, TValue> selector, IEnumerable<TValue> values);

    TBuilder NotIn<TValue>(Func<T, TValue> selector, IEnumerable<TValue> values);
}