using Shared.Enums.Time;

namespace Shared.Interfaces.Helpers;

public interface ITimeSpanHelper
{ 
    TimeSpan CreateTimeSpan(int hours, int minutes, int seconds);
    TimeSpan CreateTimeSpanFromDays(int days);
    TimeSpan CreateTimeSpanFromHours(double hours);
    TimeSpan CreateTimeSpanFromMinutes(double minutes);
    TimeSpan CreateTimeSpanFromSeconds(double seconds);
    string FormatTimeSpan(TimeSpan timeSpan, TimeSpanFormat format);
    TimeSpan AddTimeSpans(TimeSpan first, TimeSpan second);
    TimeSpan SubtractTimeSpans(TimeSpan first, TimeSpan second);

}