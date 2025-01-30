namespace Shared.Interfaces.Builders;

public interface IDateTimeCondition<out TBuilder, T>
{
    TBuilder FutureDate(Func<T, DateTime> selector);
    TBuilder PastDate(Func<T, DateTime> selector);
    TBuilder BetweenDates(Func<T, DateTime> selector, DateTime startDate, DateTime endDate);
    TBuilder BetweenDates(Func<T, DateTime> selector, Func<T, DateTime> startDateSelector, Func<T, DateTime> endDateSelector);
}