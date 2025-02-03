namespace Shared.Interfaces.Builders;

public interface IDateTimeCondition<out TBuilder, T>
{
    TBuilder FutureDate(Func<T, DateTime> selector);
    TBuilder PastDate(Func<T, DateTime> selector);
    TBuilder BetweenDates(Func<T, DateTime> selector, DateTime startDate, DateTime endDate);
    TBuilder BetweenDates(Func<T, DateTime> selector, Func<T, DateTime> startDateSelector, Func<T, DateTime> endDateSelector);
    TBuilder ExactDate(Func<T, DateTime> selector, DateTime date);
     TBuilder BeforeDate(Func<T, DateTime> selector, DateTime date);
     TBuilder AfterDate(Func<T, DateTime> selector, DateTime date);
     TBuilder IsToday(Func<T, DateTime> selector);
     TBuilder IsYesterday(Func<T, DateTime> selector);
     TBuilder IsTomorrow(Func<T, DateTime> selector);
     TBuilder InLastDays(Func<T, DateTime> selector, int days);
     TBuilder InNextDays(Func<T, DateTime> selector, int days);
     TBuilder IsWeekend(Func<T, DateTime> selector);
     TBuilder IsWeekday(Func<T, DateTime> selector);
     TBuilder IsLeapYear(Func<T, DateTime> selector);
     TBuilder SameMonthAs(Func<T, DateTime> selector, DateTime date);
     TBuilder SameYearAs(Func<T, DateTime> selector, DateTime date);


}