namespace Shared.Interfaces.Builders;

public interface ICondition<out TBuilder, T>
{
    bool Build(T obj);
    TBuilder Add(Func<T, bool> condition);
}