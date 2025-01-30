using System.Linq.Expressions;

namespace Shared.Interfaces.Builders;

public interface INumberCondition<out TBuilder, T>
{
    TBuilder Zero(Func<T, int> selector);
    TBuilder Zero(Func<T, long> selector);
    TBuilder Zero(Func<T, float> selector);
    TBuilder Zero(Func<T, double> selector);
    TBuilder Zero(Func<T, decimal> selector);
    TBuilder Zero(Func<T, short> selector);

    TBuilder NotZero(Func<T, int> selector);
    TBuilder NotZero(Func<T, long> selector);
    TBuilder NotZero(Func<T, float> selector);
    TBuilder NotZero(Func<T, double> selector);
    TBuilder NotZero(Func<T, decimal> selector);
    TBuilder NotZero(Func<T, short> selector);


    TBuilder InRange(Func<T, int> selector, int min, int max);
    TBuilder InRange(Func<T, long> selector, long min, long max);

    TBuilder InRange(Func<T, float> selector, float min, float max);
    TBuilder InRange(Func<T, double> selector, double min, double max);
    TBuilder InRange(Func<T, decimal> selector, decimal min, decimal max);
    TBuilder InRange(Func<T, short> selector, short min, short max);

    TBuilder InRange(Expression<Func<T, int>> selector, Expression<Func<T, int>> minSelector,
        Expression<Func<T, int>> maxSelector);

    TBuilder InRange(Expression<Func<T, long>> selector, Expression<Func<T, long>> minSelector,
        Expression<Func<T, long>> maxSelector);

    TBuilder InRange(Expression<Func<T, float>> selector, Expression<Func<T, float>> minSelector,
        Expression<Func<T, float>> maxSelector);

    TBuilder InRange(Expression<Func<T, double>> selector, Expression<Func<T, double>> minSelector,
        Expression<Func<T, double>> maxSelector);

    TBuilder InRange(Expression<Func<T, decimal>> selector, Expression<Func<T, decimal>> minSelector,
        Expression<Func<T, decimal>> maxSelector); 
    TBuilder InRange(Expression<Func<T, short>> selector, Expression<Func<T, short>> minSelector,
        Expression<Func<T, short>> maxSelector);


    TBuilder GreaterThan(Func<T, int> selector, int value);
    TBuilder GreaterThan(Func<T, long> selector, long value);
    TBuilder GreaterThan(Func<T, float> selector, float value);
    TBuilder GreaterThan(Func<T, double> selector, double value);
    TBuilder GreaterThan(Func<T, decimal> selector, decimal value);
    TBuilder GreaterThan(Func<T, short> selector, short value);

    TBuilder GreaterThanOrEqualTo(Func<T, int> selector, int value);
    TBuilder GreaterThanOrEqualTo(Func<T, long> selector, long value);
    TBuilder GreaterThanOrEqualTo(Func<T, float> selector, float value);
    TBuilder GreaterThanOrEqualTo(Func<T, double> selector, double value);
    TBuilder GreaterThanOrEqualTo(Func<T, decimal> selector, decimal value);
    TBuilder GreaterThanOrEqualTo(Func<T, short> selector, short value);

    TBuilder LessThan(Func<T, int> selector, int value);
    TBuilder LessThan(Func<T, long> selector, long value);
    TBuilder LessThan(Func<T, float> selector, float value);
    TBuilder LessThan(Func<T, double> selector, double value);
    TBuilder LessThan(Func<T, decimal> selector, decimal value);
    TBuilder LessThan(Func<T, short> selector, short value);

    TBuilder LessThanOrEqualTo(Func<T, int> selector, int value);
    TBuilder LessThanOrEqualTo(Func<T, long> selector, long value);
    TBuilder LessThanOrEqualTo(Func<T, float> selector, float value);
    TBuilder LessThanOrEqualTo(Func<T, double> selector, double value);
    TBuilder LessThanOrEqualTo(Func<T, decimal> selector, decimal value);
    TBuilder LessThanOrEqualTo(Func<T, short> selector, short value);

    TBuilder Positive(Func<T, int> selector);
    TBuilder Positive(Func<T, long> selector);
    TBuilder Positive(Func<T, float> selector);
    TBuilder Positive(Func<T, double> selector);
    TBuilder Positive(Func<T, decimal> selector, decimal value);
    TBuilder Positive(Func<T, short> selector, short value);

    TBuilder Negative(Func<T, int> selector);
    TBuilder Negative(Func<T, long> selector);
    TBuilder Negative(Func<T, float> selector);
    TBuilder Negative(Func<T, double> selector);
    TBuilder Negative(Func<T, decimal> selector, decimal value);
    TBuilder Negative(Func<T, short> selector, short value);

    TBuilder MinValue(Func<T, int> selector, int minValue);
    TBuilder MinValue(Func<T, long> selector, long minValue);
    TBuilder MinValue(Func<T, float> selector, float minValue);
    TBuilder MinValue(Func<T, double> selector, double minValue);
    TBuilder MinValue(Func<T, decimal> selector, decimal minValue);
    TBuilder MinValue(Func<T, short> selector, short minValue);

    TBuilder MaxValue(Func<T, int> selector, int maxValue);
    TBuilder MaxValue(Func<T, long> selector, long maxValue);
    TBuilder MaxValue(Func<T, float> selector, float maxValue);
    TBuilder MaxValue(Func<T, double> selector, double maxValue);
    TBuilder MaxValue(Func<T, decimal> selector, decimal maxValue);
    TBuilder MaxValue(Func<T, short> selector, short maxValue);
}