using Shared.Constants;
using Shared.Enums;
using Shared.Helpers;

namespace Shared.Providers;

public static class TimeSpanFormatProvider
{
    private static readonly Dictionary<TimeSpanFormat, string> TimeSpanFormats = new()
    {
        { TimeSpanFormat.Long, TimeSpanFormatConstants.Long },
        { TimeSpanFormat.Short, TimeSpanFormatConstants.Short },
        { TimeSpanFormat.Standard, TimeSpanFormatConstants.Standard },
        { TimeSpanFormat.WithDays, TimeSpanFormatConstants.WithDays },
        { TimeSpanFormat.MinutesSeconds, TimeSpanFormatConstants.MinutesSeconds }
    };

    public static string GetTimeSpanFormat(TimeSpanFormat timeSpanFormat)
    {
        if (TimeSpanFormats.TryGetValue(timeSpanFormat, out var getTimeSpanFormat))
            return getTimeSpanFormat;

        throw new ArgumentOutOfRangeException(nameof(timeSpanFormat), "formato timeSpan no soportado.");
    }

    public static TimeSpanFormat GetTimeSpanFormat(string timeSpanFormat)
    {
        var mapping =
            TimeSpanFormats.FirstOrDefault(kv => kv.Value.Equals(timeSpanFormat, StringComparison.OrdinalIgnoreCase));
        if (ValidationHelper.IsNotNull<TimeSpanFormat>(mapping.Key) && ValidationHelper.IsNotNull(mapping.Value))
            return mapping.Key;

        throw new ArgumentOutOfRangeException(nameof(timeSpanFormat), "formato timeSpan no soportado.");
    }
    
    public static IEnumerable<string> GetAllTimeSpanFormats()
    {
        return TimeSpanFormats.Values;
    }
}