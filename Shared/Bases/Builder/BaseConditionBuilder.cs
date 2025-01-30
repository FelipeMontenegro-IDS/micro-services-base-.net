using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Shared.Interfaces.Builders;
using Shared.RegularExpressions;

namespace Shared.Bases.Builder;

public abstract class BaseConditionBuilder<TBuilder, T> : ICondition<TBuilder, T>
    where TBuilder : BaseConditionBuilder<TBuilder, T>, new()
{
    private readonly HashSet<Expression<Func<T, bool>>> _conditions = new();
    private Expression<Func<T, bool>>? _orCondition;
    private bool _isAnd = true;

    public TBuilder Add(Expression<Func<T, bool>> condition)
    {
        if (condition == null) throw new ArgumentNullException(nameof(condition));
        EnsureValidCondition(condition);

        if (_isAnd)
        {
            _conditions.Add(condition);
        }
        else
        {
            _orCondition = _orCondition is null
                ? condition
                : Or(_orCondition, condition);
        }

        return (TBuilder)this;
    }

    public TBuilder Add<TValue>(Expression<Func<T, TValue>> selector, Expression<Func<TValue, bool>> predicate)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        EnsureValidCondition(predicate);

        var parameter = Expression.Parameter(typeof(T));
        var selectorBody = Expression.Invoke(selector, parameter);
        var predicateBody = Expression.Invoke(predicate, selectorBody);

        var combinedCondition = Expression.Lambda<Func<T, bool>>(predicateBody, parameter);

        return Add(combinedCondition);
    }

    public TBuilder AddSubGroup(Action<TBuilder> groupBuilder)
    {
        TBuilder groupBuilderInstance = new TBuilder();
        groupBuilder(groupBuilderInstance);

        // Construir la condición del grupo
        Expression<Func<T, bool>> groupCondition = groupBuilderInstance.Build();

        // Verificar si la condición es trivial (si es null o una constante true)
        if (groupCondition.Body is ConstantExpression constant && constant.Value is bool value && value)
        {
            return (TBuilder)this; // No agrega condiciones triviales (si la condición es true o null).
        }

        // Si la condición no es trivial, agregarla
        return Add(groupCondition);
    }

    public bool Evaluate(T obj)
    {
        try
        {
            Func<T, bool> compiledCondition = Build().Compile(); // Compila la expresión internamente.
            return compiledCondition(obj);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error in evaluating the conditions.", ex);
        }
    }

    public IEnumerable<T> EvaluateAll(IEnumerable<T> collection)
    {
        if (collection == null || !collection.Any())
        {
            throw new ArgumentException("La colección no puede ser nula o vacía.", nameof(collection));
        }

        Func<T, bool> compiledCondition = Build().Compile();
        return collection.Where(compiledCondition);
    }

    public TBuilder And()
    {
        _isAnd = true;
        return (TBuilder)this;
    }

    public TBuilder Or()
    {
        _isAnd = false;
        return (TBuilder)this;
    }

    public TBuilder NotNull(Func<T, object?> selector)
    {
        return Add(x => selector(x) != null);
    }

    public TBuilder Null(Func<T, object?> selector)
    {
        return Add(x => selector(x) == null);
    }

    public TBuilder InRange(Func<T, int> selector, int min, int max)
    {
        return Add(x => selector(x) >= min && selector(x) <= max);
    }

    public TBuilder MinLength(Func<T, string?> selector, int minLength)
    {
        return Add(x => (selector(x) ?? string.Empty).Length >= minLength);
    }

    public TBuilder MaxLength(Func<T, string?> selector, int maxLength)
    {
        return Add(x => (selector(x) ?? string.Empty).Length <= maxLength);
    }

    public TBuilder GreaterThan(Func<T, int> selector, int value)
    {
        return Add(x => selector(x) > value);
    }

    public TBuilder GreaterThanOrEqualTo(Func<T, int> selector, int value)
    {
        return Add(x => selector(x) >= value);
    }

    public TBuilder LessThan(Func<T, int> selector, int value)
    {
        return Add(x => selector(x) < value);
    }

    public TBuilder LessThanOrEqualTo(Func<T, int> selector, int value)
    {
        return Add(x => selector(x) <= value);
    }

    public TBuilder EqualTo<TValue>(Func<T, TValue> selector, TValue value) where TValue : IEquatable<TValue>
    {
        return Add(x => selector(x).Equals(value));
    }

    public TBuilder RegexMatch(Func<T, string?> selector, string pattern)
    {
        return Add(x => Regex.IsMatch(selector(x) ?? string.Empty, pattern));
    }

    public TBuilder NotEmpty(Func<T, IEnumerable<object?>> selector)
    {
        return Add(x => selector(x).Any() == true);
    }

    public TBuilder IsTrue(Func<T, bool> selector)
    {
        return Add(x => selector(x) == true);
    }

    public TBuilder IsFalse(Func<T, bool> selector)
    {
        return Add(x => selector(x) == false);
    }

    public TBuilder IsEmpty(Func<T, string?> selector)
    {
        return Add(x => string.IsNullOrEmpty(selector(x)));
    }

    public TBuilder IsNotEmpty(Func<T, string?> selector)
    {
        return Add(x => !string.IsNullOrEmpty(selector(x)));
    }

    public TBuilder IsFutureDate(Func<T, DateTime> selector)
    {
        return Add(x => selector(x) > DateTime.Now);
    }

    public TBuilder IsPastDate(Func<T, DateTime> selector)
    {
        return Add(x => selector(x) < DateTime.Now);
    }

    public TBuilder IsPositive(Func<T, int> selector)
    {
        return Add(x => selector(x) > 0);
    }

    public TBuilder IsNegative(Func<T, int> selector)
    {
        return Add(x => selector(x) < 0);
    }

    public TBuilder NotEqualTo<TValue>(Func<T, TValue> selector, TValue value)  where TValue : IEquatable<TValue>
    {
        return Add(x => !selector(x).Equals(value));
    }

    public TBuilder MinValue(Func<T, int> selector, int minValue)
    {
        return Add(x => selector(x) >= minValue);
    }

    public TBuilder MaxValue(Func<T, int> selector, int maxValue)
    {
        return Add(x => selector(x) <= maxValue);
    }

    public TBuilder Email(Func<T, string?> selector)
    {
        return Add(x => RegularExpression.FormatEmail.IsMatch(selector(x) ?? string.Empty));
    }

    public TBuilder InRange(Expression<Func<T, int>> selector, Expression<Func<T, int>> minSelector,
        Expression<Func<T, int>> maxSelector)
    {
        Expression<Func<T, bool>> filter = Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(
                Expression.GreaterThanOrEqual(selector.Body, minSelector.Body),
                Expression.LessThanOrEqual(selector.Body, maxSelector.Body)
            ),
            selector.Parameters
        );
        return Add(filter);
    }

    public TBuilder In<TValue>(Func<T, TValue> selector, IEnumerable<TValue> values)
    {
        var valueSet = values.ToHashSet();
        return Add(x => valueSet.Contains(selector(x)));
    }

    public TBuilder NotIn<TValue>(Func<T, TValue> selector, IEnumerable<TValue> values)
    {
        var valueSet = values.ToHashSet();
        return Add(x => !valueSet.Contains(selector(x)));
    }

    public TBuilder BetweenDates(Func<T, DateTime> selector, DateTime startDate, DateTime endDate)
    {
        return Add(x => selector(x) >= startDate && selector(x) <= endDate);
    }

    public TBuilder BetweenDates(Func<T, DateTime> selector, Func<T, DateTime> startDateSelector,
        Func<T, DateTime> endDateSelector)
    {
        return Add(x => selector(x) >= startDateSelector(x) && selector(x) <= endDateSelector(x));
    }

    public TBuilder EndsWith(Func<T, string> selector, string value)
    {
        return Add(x => selector(x).EndsWith(value));
    }

    public TBuilder StartsWith(Func<T, string> selector, string value)
    {
        return Add(x => selector(x).EndsWith(value));
    }

    public TBuilder Contains(Func<T, string> selector, string value)
    {
        return Add(x => selector(x).Contains(value));
    }

    /// <summary>
    /// Combina dos expresiones de tipo <see>
    ///     <cref>Expression{Func{T, bool}}</cref>
    /// </see>
    /// utilizando una operación lógica "AND".
    /// </summary>
    /// <param name="first">La primera expresión a combinar.</param>
    /// <param name="second">La segunda expresión a combinar.</param>
    /// <returns>Una nueva expresión que representa la combinación de ambas expresiones con un "AND" lógico.</returns>
    /// <exception cref="ArgumentNullException">Lanza si alguna de las expresiones proporcionadas es nula.</exception>
    private Expression<Func<T, bool>> And(Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        ParameterExpression parameter = Expression.Parameter(typeof(T));
        BinaryExpression body =
            Expression.AndAlso(Expression.Invoke(first, parameter), Expression.Invoke(second, parameter));
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }


    /// <summary>
    /// Combina dos expresiones de tipo <see>
    ///     <cref>Expression{Func{T, bool}}</cref>
    /// </see>
    /// utilizando una operación lógica "OR".
    /// </summary>
    /// <param name="first">La primera expresión a combinar.</param>
    /// <param name="second">La segunda expresión a combinar.</param>
    /// <returns>Una nueva expresión que representa la combinación de ambas expresiones con un "OR" lógico.</returns>
    /// <exception cref="ArgumentNullException">Lanza si alguna de las expresiones proporcionadas es nula.</exception>
    private Expression<Func<T, bool>> Or(Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.OrElse(
            Expression.Invoke(first, parameter),
            Expression.Invoke(second, parameter)
        );

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    private void EnsureValidCondition(Expression<Func<T, bool>> condition)
    {
        switch (condition.Body)
        {
            // Verificar si la condición es una constante que siempre es 'true'
            case ConstantExpression constant when constant.Value is bool value && value:
                throw new ArgumentException("The condition provided has no effect because it is always 'true'.");
            // Verificar si la condición es una constante que siempre es 'false'
            case ConstantExpression constantFalse when constantFalse.Value is bool valueFalse && !valueFalse:
                throw new ArgumentException("The condition provided has no effect because it is always 'false'.");
            // Verificar si la condición es una constante de tipo nulo (como x => null)
            case ConstantExpression constantNull when constantNull.Value == null:
                throw new ArgumentException("The condition provided has no effect because it is always 'null'.");
            // Verificar si la condición está comparando a '0' (ejemplo: x => 0)
            case BinaryExpression binaryExpression when
                binaryExpression.Left is ConstantExpression leftConstant &&
                leftConstant.Value is int leftValue && leftValue == 0:
                throw new ArgumentException("The condition provided has no effect because it is always '0'.");
        }
    }

    private void EnsureValidCondition<TValue>(Expression<Func<TValue, bool>> condition)
    {
        switch (condition.Body)
        {
            // Verificar si la condición es una constante que siempre es 'true'
            case ConstantExpression constant when constant.Value is bool value && value:
                throw new ArgumentException("The condition provided has no effect because it is always 'true'.");
            // Verificar si la condición es una constante que siempre es 'false'
            case ConstantExpression constantFalse when constantFalse.Value is bool valueFalse && !valueFalse:
                throw new ArgumentException("The condition provided has no effect because it is always 'false'.");
            // Verificar si la condición es una constante de tipo nulo (como x => null)
            case ConstantExpression constantNull when constantNull.Value == null:
                throw new ArgumentException("The condition provided has no effect because it is always 'null'.");
            // Verificar si la condición está comparando a '0' (ejemplo: x => 0)
            case BinaryExpression binaryExpression when
                binaryExpression.Left is ConstantExpression leftConstant &&
                leftConstant.Value is int leftValue && leftValue == 0:
                throw new ArgumentException("The condition provided has no effect because it is always '0'.");
        }
    }

    public Expression<Func<T, bool>> Build()
    {
        if (!_conditions.Any() && _orCondition is null)
        {
            return _ => true; // Devuelve una expresión que siempre es true si no hay condiciones.
        }

        var andCondition = _conditions
            .DefaultIfEmpty(_ => true) // Si no hay condiciones, devuelve una que siempre es true.
            .Aggregate(And);

        return _orCondition is null ? andCondition : Or(andCondition, _orCondition);
    }
}