using Shared.Interfaces.Builders;

namespace Shared.Bases.Builder;

public abstract class BaseConditionBuilder<TBuilder, T> : ICondition<TBuilder, T>
    where TBuilder : BaseConditionBuilder<TBuilder, T>
{
    private readonly HashSet<Func<T, bool>> _conditions = new();
    private bool _isAnd = true; // Por defecto, se asume que se están usando condiciones AND

    public TBuilder Add(Func<T, bool> validation)
    {
        if (_isAnd)
        {
            _conditions.Add(validation);
        }
        else
        {
            var previousConditions = _conditions.ToList();
            _conditions.Clear();
            _conditions.Add(obj => previousConditions.Any(prevCondition => prevCondition(obj)) || validation(obj));
        }

        return (TBuilder)this;
    }

    public TBuilder And()
    {
        _isAnd = true;
        return  (TBuilder)this;
    }

    public TBuilder Or()
    {
        _isAnd = false;
        return  (TBuilder)this;
    }

    public bool Build(T obj)
    {
        return _conditions.All(condition => condition(obj));
    }
}