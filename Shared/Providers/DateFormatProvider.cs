using Shared.Constants;
using Shared.Enums;
using Shared.Helpers;

namespace Shared.Providers;

public static class DateFormatProvider
{
    private static readonly Dictionary<DateFormat, string> DateFormats = new()
    {
        { DateFormat.LongDate, DateFormatConstants.LongDate },
        { DateFormat.LongTime, DateFormatConstants.LongTime },
        { DateFormat.ShortDate, DateFormatConstants.ShortDate },
        { DateFormat.ShortTime, DateFormatConstants.ShortTime },
        { DateFormat.IsoDate, DateFormatConstants.IsoDate },
        { DateFormat.FullDateTime, DateFormatConstants.FullDateTime }
    };

    public static string GetDateFormat(DateFormat dateFormat)
    {
        return DateFormats.GetValueOrDefault(dateFormat, DateFormatConstants.IsoDate);
    }

    public static DateFormat GetDateFormat(string dateFormat)
    {
        var mapping = DateFormats.FirstOrDefault(kv => kv.Value.Equals(dateFormat, StringComparison.OrdinalIgnoreCase));
        if (ValidationHelper.IsNotNull<DateFormat>(mapping.Key) && ValidationHelper.IsNotNull(mapping.Value))
            return mapping.Key;

        return DateFormat.IsoDate;
    }
    
    public static IEnumerable<string> GetAllDateFormats()
    {
        return DateFormats.Values;
    }
}