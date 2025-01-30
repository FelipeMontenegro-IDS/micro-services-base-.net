namespace Shared.Interfaces.Builders;

public interface IBoolCondition<out TBuilder, T>
{
    TBuilder IsTrue(Func<T, bool> selector);

    TBuilder IsFalse(Func<T, bool> selector);
}