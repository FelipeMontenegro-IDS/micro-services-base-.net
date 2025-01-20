using Shared.Interfaces.Builders;

namespace Shared.Bases.Builder;

public abstract class BaseConditionBuilder<TBuilder, T> : ICondition<TBuilder, T>
    where TBuilder : BaseConditionBuilder<TBuilder, T>
{
    private readonly HashSet<Func<T, bool>> _conditions = new();

    public TBuilder Add(Func<T, bool> validation)
    {
        _conditions.Add(validation);
        return (TBuilder)this;
    }

    public bool Build(T obj)
    {
        return _conditions.All(condition => condition(obj));
    }
}