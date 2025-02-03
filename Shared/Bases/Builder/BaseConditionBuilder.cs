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

        ParameterExpression parameter = Expression.Parameter(typeof(T), "Type");
        InvocationExpression selectorBody = Expression.Invoke(selector, parameter);
        InvocationExpression predicateBody = Expression.Invoke(predicate, selectorBody);

        Expression<Func<T, bool>> combinedCondition = Expression.Lambda<Func<T, bool>>(predicateBody, parameter);

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

    public TBuilder NotNull(Expression<Func<T, object?>> selector)
    {
        Expression<Func<object?, bool>> predicate = value => value != null;
        return Add(selector, predicate);
    }

    public TBuilder Null(Expression<Func<T, object?>> selector)
    {
        Expression<Func<object?, bool>> predicate = value => value == null;
        return Add(selector, predicate);
    }

    public TBuilder NotZero(Expression<Func<T, decimal>> selector)
    {
        Expression<Func<decimal, bool>> predicate = val => val != 0m;
        return Add(selector, predicate);
    }

    public TBuilder NotZero(Expression<Func<T, short>> selector)
    {
        Expression<Func<short, bool>> predicate = val => val != 0;
        return Add(selector, predicate);
    }

    public TBuilder InRange(Func<T, int> selector, int min, int max)
    {
        return Add(x => selector(x) >= min && selector(x) <= max);
    }

    public TBuilder InRange(Func<T, long> selector, long min, long max)
    {
        return Add(x => selector(x) >= min && selector(x) <= max);
    }

    public TBuilder InRange(Func<T, float> selector, float min, float max)
    {
        return Add(x => selector(x) >= min && selector(x) <= max);
    }

    public TBuilder InRange(Func<T, double> selector, double min, double max)
    {
        return Add(x => selector(x) >= min && selector(x) <= max);
    }

    public TBuilder InRange(Func<T, decimal> selector, decimal min, decimal max)
    {
        return Add(x => selector(x) >= min && selector(x) <= max);
    }

    public TBuilder InRange(Func<T, short> selector, short min, short max)
    {
        return Add(x => selector(x) >= min && selector(x) <= max);
    }

    public TBuilder MinLength(Expression<Func<T, string?>> selector, int minLength)
    {
        Expression<Func<string?, bool>> predicate = val => !string.IsNullOrEmpty(val) && val.Length >= minLength;
        return Add(selector, predicate);
    }

    public TBuilder MaxLength(Expression<Func<T, string?>> selector, int maxLength)
    {
        Expression<Func<string?, bool>> predicate = val => !string.IsNullOrEmpty(val) && val.Length <= maxLength;
        return Add(selector, predicate);
    }

    public TBuilder InRange(Expression<Func<T, decimal>> selector, Expression<Func<T, decimal>> minSelector,
        Expression<Func<T, decimal>> maxSelector)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));
        if (minSelector == null) throw new ArgumentNullException(nameof(minSelector));
        if (maxSelector == null) throw new ArgumentNullException(nameof(maxSelector));

        ParameterExpression parameter = Expression.Parameter(typeof(T), "Type");

        InvocationExpression selectorBody = Expression.Invoke(selector, parameter);
        InvocationExpression minSelectorBody = Expression.Invoke(minSelector, parameter);
        InvocationExpression maxSelectorBody = Expression.Invoke(maxSelector, parameter);

        Expression greaterThanOrEqual = Expression.GreaterThanOrEqual(selectorBody, minSelectorBody);
        Expression lessThanOrEqual = Expression.LessThanOrEqual(selectorBody, maxSelectorBody);
        Expression finalExpression = Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);

        Expression<Func<T, bool>> filter = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);

        return Add(filter);
    }

    public TBuilder InRange(Expression<Func<T, short>> selector, Expression<Func<T, short>> minSelector,
        Expression<Func<T, short>> maxSelector)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));
        if (minSelector == null) throw new ArgumentNullException(nameof(minSelector));
        if (maxSelector == null) throw new ArgumentNullException(nameof(maxSelector));

        ParameterExpression parameter = Expression.Parameter(typeof(T), "Type");

        InvocationExpression selectorBody = Expression.Invoke(selector, parameter);
        InvocationExpression minSelectorBody = Expression.Invoke(minSelector, parameter);
        InvocationExpression maxSelectorBody = Expression.Invoke(maxSelector, parameter);

        Expression greaterThanOrEqual = Expression.GreaterThanOrEqual(selectorBody, minSelectorBody);
        Expression lessThanOrEqual = Expression.LessThanOrEqual(selectorBody, maxSelectorBody);
        Expression finalExpression = Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);

        Expression<Func<T, bool>> filter = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);

        return Add(filter);
    }

    public TBuilder GreaterThan(Func<T, int> selector, int value)
    {
        return Add(x => selector(x) > value);
    }

    public TBuilder GreaterThan(Func<T, long> selector, long value)
    {
        return Add(x => selector(x) > value);
    }

    public TBuilder GreaterThan(Func<T, float> selector, float value)
    {
        return Add(x => selector(x) > value);
    }

    public TBuilder GreaterThan(Func<T, double> selector, double value)
    {
        return Add(x => selector(x) > value);
    }

    public TBuilder GreaterThan(Func<T, decimal> selector, decimal value)
    {
        return Add(x => selector(x) > value);
    }

    public TBuilder GreaterThan(Func<T, short> selector, short value)
    {
        return Add(x => selector(x) > value);
    }

    public TBuilder GreaterThanOrEqualTo(Func<T, int> selector, int value)
    {
        return Add(x => selector(x) >= value);
    }

    public TBuilder GreaterThanOrEqualTo(Func<T, long> selector, long value)
    {
        return Add(x => selector(x) >= value);
    }

    public TBuilder GreaterThanOrEqualTo(Func<T, float> selector, float value)
    {
        return Add(x => selector(x) >= value);
    }

    public TBuilder GreaterThanOrEqualTo(Func<T, double> selector, double value)
    {
        return Add(x => selector(x) >= value);
    }

    public TBuilder GreaterThanOrEqualTo(Func<T, decimal> selector, decimal value)
    {
        return Add(x => selector(x) >= value);
    }

    public TBuilder GreaterThanOrEqualTo(Func<T, short> selector, short value)
    {
        return Add(x => selector(x) >= value);
    }

    public TBuilder LessThan(Func<T, int> selector, int value)
    {
        return Add(x => selector(x) < value);
    }

    public TBuilder LessThan(Func<T, long> selector, long value)
    {
        return Add(x => selector(x) < value);
    }

    public TBuilder LessThan(Func<T, float> selector, float value)
    {
        return Add(x => selector(x) < value);
    }

    public TBuilder LessThan(Func<T, double> selector, double value)
    {
        return Add(x => selector(x) < value);
    }

    public TBuilder LessThan(Func<T, decimal> selector, decimal value)
    {
        return Add(x => selector(x) < value);
    }

    public TBuilder LessThan(Func<T, short> selector, short value)
    {
        return Add(x => selector(x) < value);
    }

    public TBuilder LessThanOrEqualTo(Func<T, int> selector, int value)
    {
        return Add(x => selector(x) <= value);
    }

    public TBuilder LessThanOrEqualTo(Func<T, long> selector, long value)
    {
        return Add(x => selector(x) <= value);
    }

    public TBuilder LessThanOrEqualTo(Func<T, float> selector, float value)
    {
        return Add(x => selector(x) <= value);
    }

    public TBuilder LessThanOrEqualTo(Func<T, double> selector, double value)
    {
        return Add(x => selector(x) <= value);
    }

    public TBuilder LessThanOrEqualTo(Func<T, decimal> selector, decimal value)
    {
        return Add(x => selector(x) <= value);
    }

    public TBuilder LessThanOrEqualTo(Func<T, short> selector, short value)
    {
        return Add(x => selector(x) <= value);
    }

    public TBuilder EqualTo<TValue>(Expression<Func<T, TValue>> selector, TValue value)
        where TValue : IEquatable<TValue>
    {
        Expression<Func<TValue, bool>> predicate = v => v.Equals(value);
        return Add(selector, predicate);
    }

    public TBuilder RegexMatch(Expression<Func<T, string?>> selector, string pattern)
    {
        if (string.IsNullOrEmpty(pattern)) throw new ArgumentException("Pattern cannot be empty", nameof(pattern));

        Expression<Func<string?, bool>> predicate = value =>
            !string.IsNullOrEmpty(value) && Regex.IsMatch(value, pattern);
        return Add(selector, predicate);
    }

    public TBuilder NotEmpty(Expression<Func<T, IEnumerable<object?>>> selector)
    {
        Expression<Func<IEnumerable<object?>, bool>> predicate = val => val.Any() == true;
        return Add(selector, predicate);
    }

    public TBuilder IsTrue(Func<T, bool> selector)
    {
        return Add(x => selector(x) == true);
    }

    public TBuilder IsFalse(Func<T, bool> selector)
    {
        return Add(x => selector(x) == false);
    }

    public TBuilder Empty(Expression<Func<T, string?>> selector)
    {
        Expression<Func<string?, bool>> predicate = val => string.IsNullOrEmpty(val);
        return Add(selector, predicate);
    }

    public TBuilder NotEmpty(Expression<Func<T, string?>> selector)
    {
        Expression<Func<string?, bool>> predicate = val => !string.IsNullOrEmpty(val);
        return Add(selector, predicate);
    }

    public TBuilder FutureDate(Expression<Func<T, DateTime>> selector)
    {
        Expression<Func<DateTime, bool>> predicate = val => val > DateTime.Now;
        return Add(selector, predicate);
    }

    public TBuilder PastDate(Expression<Func<T, DateTime>> selector)
    {
        Expression<Func<DateTime, bool>> predicate = val => val < DateTime.Now;
        return Add(selector, predicate);
    }

    public TBuilder Positive(Func<T, int> selector)
    {
        return Add(x => selector(x) > 0);
    }

    public TBuilder Positive(Func<T, long> selector)
    {
        return Add(x => selector(x) > 0L);
    }

    public TBuilder Positive(Func<T, float> selector)
    {
        return Add(x => selector(x) > 0f);
    }

    public TBuilder Positive(Func<T, double> selector)
    {
        return Add(x => selector(x) > 0.0);
    }

    public TBuilder Positive(Func<T, decimal> selector, decimal value)
    {
        return Add(x => selector(x) > 0m);
    }

    public TBuilder Positive(Func<T, short> selector, short value)
    {
        return Add(x => selector(x) > 0);
    }

    public TBuilder Negative(Func<T, int> selector)
    {
        return Add(x => selector(x) < 0);
    }

    public TBuilder Negative(Func<T, long> selector)
    {
        return Add(x => selector(x) < 0L);
    }

    public TBuilder Negative(Func<T, float> selector)
    {
        return Add(x => selector(x) < 0f);
    }

    public TBuilder Negative(Func<T, double> selector)
    {
        return Add(x => selector(x) < 0.0);
    }

    public TBuilder Negative(Func<T, decimal> selector, decimal value)
    {
        return Add(x => selector(x) < 0m);
    }

    public TBuilder Negative(Func<T, short> selector, short value)
    {
        return Add(x => selector(x) < 0);
    }

    public TBuilder NotEqualTo<TValue>(Expression<Func<T, TValue>> selector, TValue value)
        where TValue : IEquatable<TValue>
    {
        Expression<Func<TValue, bool>> predicate = val => !val.Equals(value);
        return Add(selector, predicate);
    }

    public TBuilder MinValue(Func<T, int> selector, int minValue)
    {
        return Add(x => selector(x) >= minValue);
    }

    public TBuilder MinValue(Func<T, long> selector, long minValue)
    {
        return Add(x => selector(x) >= minValue);
    }

    public TBuilder MinValue(Func<T, float> selector, float minValue)
    {
        return Add(x => selector(x) >= minValue);
    }

    public TBuilder MinValue(Func<T, double> selector, double minValue)
    {
        return Add(x => selector(x) >= minValue);
    }

    public TBuilder MinValue(Func<T, decimal> selector, decimal minValue)
    {
        return Add(x => selector(x) >= minValue);
    }

    public TBuilder MinValue(Func<T, short> selector, short minValue)
    {
        return Add(x => selector(x) >= minValue);
    }

    public TBuilder MaxValue(Func<T, int> selector, int maxValue)
    {
        return Add(x => selector(x) <= maxValue);
    }

    public TBuilder MaxValue(Func<T, long> selector, long maxValue)
    {
        return Add(x => selector(x) <= maxValue);
    }

    public TBuilder MaxValue(Func<T, float> selector, float maxValue)
    {
        return Add(x => selector(x) <= maxValue);
    }

    public TBuilder MaxValue(Func<T, double> selector, double maxValue)
    {
        return Add(x => selector(x) <= maxValue);
    }

    public TBuilder MaxValue(Func<T, decimal> selector, decimal maxValue)
    {
        return Add(x => selector(x) <= maxValue);
    }

    public TBuilder MaxValue(Func<T, short> selector, short maxValue)
    {
        return Add(x => selector(x) <= maxValue);
    }

    public TBuilder Email(Expression<Func<T, string?>> selector)
    {
        Expression<Func<string?, bool>> predicate = val =>
            !string.IsNullOrEmpty(val) && RegularExpression.FormatEmail.IsMatch(val);
        return Add(selector, predicate);
    }

    public TBuilder InRange(Expression<Func<T, int>> selector, Expression<Func<T, int>> minSelector,
        Expression<Func<T, int>> maxSelector)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));
        if (minSelector == null) throw new ArgumentNullException(nameof(minSelector));
        if (maxSelector == null) throw new ArgumentNullException(nameof(maxSelector));

        ParameterExpression parameter = Expression.Parameter(typeof(T), "Type");

        InvocationExpression selectorBody = Expression.Invoke(selector, parameter);
        InvocationExpression minSelectorBody = Expression.Invoke(minSelector, parameter);
        InvocationExpression maxSelectorBody = Expression.Invoke(maxSelector, parameter);

        Expression greaterThanOrEqual = Expression.GreaterThanOrEqual(selectorBody, minSelectorBody);
        Expression lessThanOrEqual = Expression.LessThanOrEqual(selectorBody, maxSelectorBody);
        Expression finalExpression = Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);

        Expression<Func<T, bool>> filter = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);

        return Add(filter);
    }

    public TBuilder InRange(Expression<Func<T, long>> selector, Expression<Func<T, long>> minSelector,
        Expression<Func<T, long>> maxSelector)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));
        if (minSelector == null) throw new ArgumentNullException(nameof(minSelector));
        if (maxSelector == null) throw new ArgumentNullException(nameof(maxSelector));

        ParameterExpression parameter = Expression.Parameter(typeof(T), "Type");

        InvocationExpression selectorBody = Expression.Invoke(selector, parameter);
        InvocationExpression minSelectorBody = Expression.Invoke(minSelector, parameter);
        InvocationExpression maxSelectorBody = Expression.Invoke(maxSelector, parameter);

        Expression greaterThanOrEqual = Expression.GreaterThanOrEqual(selectorBody, minSelectorBody);
        Expression lessThanOrEqual = Expression.LessThanOrEqual(selectorBody, maxSelectorBody);
        Expression finalExpression = Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);

        Expression<Func<T, bool>> filter = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);

        return Add(filter);
    }

    public TBuilder InRange(Expression<Func<T, float>> selector, Expression<Func<T, float>> minSelector,
        Expression<Func<T, float>> maxSelector)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));
        if (minSelector == null) throw new ArgumentNullException(nameof(minSelector));
        if (maxSelector == null) throw new ArgumentNullException(nameof(maxSelector));

        ParameterExpression parameter = Expression.Parameter(typeof(T), "Type");

        InvocationExpression selectorBody = Expression.Invoke(selector, parameter);
        InvocationExpression minSelectorBody = Expression.Invoke(minSelector, parameter);
        InvocationExpression maxSelectorBody = Expression.Invoke(maxSelector, parameter);

        Expression greaterThanOrEqual = Expression.GreaterThanOrEqual(selectorBody, minSelectorBody);
        Expression lessThanOrEqual = Expression.LessThanOrEqual(selectorBody, maxSelectorBody);
        Expression finalExpression = Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);

        Expression<Func<T, bool>> filter = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);

        return Add(filter);
    }

    public TBuilder InRange(Expression<Func<T, double>> selector, Expression<Func<T, double>> minSelector,
        Expression<Func<T, double>> maxSelector)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));
        if (minSelector == null) throw new ArgumentNullException(nameof(minSelector));
        if (maxSelector == null) throw new ArgumentNullException(nameof(maxSelector));

        ParameterExpression parameter = Expression.Parameter(typeof(T), "Type");

        InvocationExpression selectorBody = Expression.Invoke(selector, parameter);
        InvocationExpression minSelectorBody = Expression.Invoke(minSelector, parameter);
        InvocationExpression maxSelectorBody = Expression.Invoke(maxSelector, parameter);

        Expression greaterThanOrEqual = Expression.GreaterThanOrEqual(selectorBody, minSelectorBody);
        Expression lessThanOrEqual = Expression.LessThanOrEqual(selectorBody, maxSelectorBody);
        Expression finalExpression = Expression.AndAlso(greaterThanOrEqual, lessThanOrEqual);

        Expression<Func<T, bool>> filter = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);

        return Add(filter);
    }

    public TBuilder In<TValue>(Expression<Func<T, TValue>> selector, IEnumerable<TValue> values)
    {
        HashSet<TValue> valueSet = values.ToHashSet();

        Expression<Func<TValue, bool>> predicate = val => valueSet.Contains(val);
        return Add(selector, predicate);
    }

    public TBuilder NotIn<TValue>(Expression<Func<T, TValue>> selector, IEnumerable<TValue> values)
    {
        HashSet<TValue> valueSet = values.ToHashSet();

        Expression<Func<TValue, bool>> predicate = val => !valueSet.Contains(val);
        return Add(selector, predicate);
    }

    public TBuilder BetweenDates(Expression<Func<T, DateTime>> selector, DateTime startDate, DateTime endDate)
    {
        Expression<Func<DateTime, bool>> predicate = val => val.Date >= startDate && val.Date <= endDate;
        return Add(selector, predicate);
    }

    public TBuilder BetweenDates(
        Expression<Func<T, DateTime>> selector,
        Expression<Func<T, DateTime>> startDateSelector,
        Expression<Func<T, DateTime>> endDateSelector)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));
        if (startDateSelector == null) throw new ArgumentNullException(nameof(startDateSelector));
        if (endDateSelector == null) throw new ArgumentNullException(nameof(endDateSelector));

        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");

        InvocationExpression selectorBody = Expression.Invoke(selector, parameter);
        InvocationExpression startDateBody = Expression.Invoke(startDateSelector, parameter);
        InvocationExpression endDateBody = Expression.Invoke(endDateSelector, parameter);

        Expression<Func<T, bool>> filter = Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(
                Expression.GreaterThanOrEqual(selectorBody, startDateBody),
                Expression.LessThanOrEqual(selectorBody, endDateBody)
            ),
            parameter
        );

        return Add(filter);
    }

    public TBuilder ExactDate(Expression<Func<T, DateTime>> selector, DateTime date)
    {
        Expression<Func<DateTime, bool>> predicate = val => val.Date == date.Date;
        return Add(selector, predicate);
    }

    public TBuilder BeforeDate(Expression<Func<T, DateTime>> selector, DateTime date)
    {
        Expression<Func<DateTime, bool>> predicate = val => val < date;
        return Add(selector, predicate);
    }

    public TBuilder AfterDate(Expression<Func<T, DateTime>> selector, DateTime date)
    {
        Expression<Func<DateTime, bool>> predicate = val => val > date;
        return Add(selector, predicate);
    }

    public TBuilder IsToday(Expression<Func<T, DateTime>> selector)
    {
        Expression<Func<DateTime, bool>> predicate = val => val.Date == DateTime.Today;
        return Add(selector, predicate);
    }

    public TBuilder IsYesterday(Expression<Func<T, DateTime>> selector)
    {
        Expression<Func<DateTime, bool>> predicate = val => val.Date == DateTime.Today.AddDays(-1);
        return Add(selector, predicate);
    }

    public TBuilder IsTomorrow(Expression<Func<T, DateTime>> selector)
    {
        Expression<Func<DateTime, bool>> predicate = val => val.Date == DateTime.Today.AddDays(1);
        return Add(selector, predicate);
    }

    public TBuilder InLastDays(Expression<Func<T, DateTime>> selector, int days)
    {
        if (days <= 0)
            throw new ArgumentOutOfRangeException(
                $"Not Permited days in zero and days necesary in positive {nameof(days)}");

        Expression<Func<DateTime, bool>> predicate = val => val.Date >= DateTime.Today.AddDays(-days);
        return Add(selector, predicate);
    }

    public TBuilder InNextDays(Expression<Func<T, DateTime>> selector, int days)
    {
        if (days <= 0)
            throw new ArgumentOutOfRangeException(
                $"Not Permited days in zero and days necesary in positive {nameof(days)}");

        Expression<Func<DateTime, bool>> predicate = val => val.Date <= DateTime.Today.AddDays(days);
        return Add(selector, predicate);
    }

    public TBuilder IsWeekend(Expression<Func<T, DateTime>> selector)
    {
        Expression<Func<DateTime, bool>> predicate = val =>
            val.DayOfWeek == DayOfWeek.Saturday || val.DayOfWeek == DayOfWeek.Sunday;
        return Add(selector, predicate);
    }

    public TBuilder IsWeekday(Expression<Func<T, DateTime>> selector)
    {
        Expression<Func<DateTime, bool>> predicate = val =>
            val.DayOfWeek >= DayOfWeek.Monday && val.DayOfWeek <= DayOfWeek.Friday;
        return Add(selector, predicate);
    }

    public TBuilder IsLeapYear(Expression<Func<T, DateTime>> selector)
    {
        Expression<Func<DateTime, bool>> predicate = val => DateTime.IsLeapYear(val.Year);
        return Add(selector, predicate);
    }

    public TBuilder SameMonthAs(Expression<Func<T, DateTime>> selector, DateTime date)
    {
        Expression<Func<DateTime, bool>> predicate = val => val.Year == date.Year && val.Month == date.Month;
        return Add(selector, predicate);
    }

    public TBuilder SameYearAs(Expression<Func<T, DateTime>> selector, DateTime date)
    {
        Expression<Func<DateTime, bool>> predicate = val => val.Year == date.Year;
        return Add(selector, predicate);
    }

    public TBuilder EndsWith(Expression<Func<T, string>> selector, string value)
    {
        Expression<Func<string, bool>> predicate = val => val.EndsWith(value);
        return Add(selector, predicate);
    }

    public TBuilder StartsWith(Expression<Func<T, string>> selector, string value)
    {
        Expression<Func<string, bool>> predicate = val => val.StartsWith(value);
        return Add(selector, predicate);
    }

    public TBuilder Contains(Expression<Func<T, string>> selector, string value)
    {
        Expression<Func<string, bool>> predicate = val => val.Contains(value);
        return Add(selector, predicate);
    }

    public TBuilder Zero(Expression<Func<T, int>> selector)
    {
        Expression<Func<int, bool>> predicate = val => val == 0;
        return Add(selector, predicate);
    }

    public TBuilder Zero(Expression<Func<T, long>> selector)
    {
        Expression<Func<long, bool>> predicate = val => val == 0L;
        return Add(selector, predicate);
    }

    public TBuilder Zero(Expression<Func<T, float>> selector)
    {
        Expression<Func<float, bool>> predicate = val => val == 0f;
        return Add(selector, predicate);
    }

    public TBuilder Zero(Expression<Func<T, double>> selector)
    {
        Expression<Func<double, bool>> predicate = val => val == 0.0;
        return Add(selector, predicate);
    }

    public TBuilder Zero(Expression<Func<T, decimal>> selector)
    {
        Expression<Func<decimal, bool>> predicate = val => val == 0m;
        return Add(selector, predicate);
    }

    public TBuilder Zero(Expression<Func<T, short>> selector)
    {
        Expression<Func<short, bool>> predicate = val => val == 0;
        return Add(selector, predicate);
    }

    public TBuilder NotZero(Expression<Func<T, int>> selector)
    {
        Expression<Func<int, bool>> predicate = val => val != 0;
        return Add(selector, predicate);
    }

    public TBuilder NotZero(Expression<Func<T, long>> selector)
    {
        Expression<Func<long, bool>> predicate = val => val != 0L;
        return Add(selector, predicate);
    }

    public TBuilder NotZero(Expression<Func<T, float>> selector)
    {
        Expression<Func<float, bool>> predicate = val => val != 0f;
        return Add(selector, predicate);
    }

    public TBuilder NotZero(Expression<Func<T, double>> selector)
    {
        Expression<Func<double, bool>> predicate = val => val != 0.0;
        return Add(selector, predicate);
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
}