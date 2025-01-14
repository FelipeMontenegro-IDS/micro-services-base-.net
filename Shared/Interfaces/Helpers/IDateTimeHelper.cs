using Shared.Enums;

namespace Shared.Interfaces.Helpers;

public interface IDateTimeHelper
{
    DateTime GetCurrentUtcDateTime();
    DateTime ConvertTo(DateTime utcDateTime, TimeZoneOption timeZoneOption);
    DateTimeOffset ConvertTo(DateTimeOffset utcDateTime, TimeZoneOption timeZoneOption);
    DateTime GetCurrentDateTimeInTimeZone(TimeZoneOption timeZoneOption);
    string FormatDate(DateTime dateTime, DateFormat format);
    int DaysBetween(DateTime startDate, DateTime endDate);
    bool IsWeekend(DateTime dateTime);
    DateTime AddDays(DateTime dateTime, int days);
    DateTime SubtractDays(DateTime dateTime, int days);
    DateTime GetFirstDayOfMonth(DateTime dateTime);
    DateTime GetLastDayOfMonth(DateTime dateTime);
    bool IsWeekday(DateTime dateTime);
    int WeeksBetween(DateTime startDate, DateTime endDate);
    DateTime GetStartOfWeek(DateTime dateTime);
    DateTime GetEndOfWeek(DateTime dateTime);
    bool IsDateInRange(DateTime dateTime, DateTime startDate, DateTime endDate);
    int GetDaysInMonth(int year, int month);
    DateTime ChangeTimeZone(DateTime dateTime, TimeZoneOption timeZoneOption, TimeZoneInfo newTimeZone);


}