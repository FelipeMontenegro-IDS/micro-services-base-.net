using Shared.Constants;
using Shared.Enums;
using Shared.Helpers;

namespace Shared.Providers;

public class DateFormatProvider
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
        if (DateFormats.TryGetValue(dateFormat, out var getDateFormat))
            return getDateFormat;

        throw new ArgumentOutOfRangeException(nameof(dateFormat), "formato de fecha no soportado.");
    }

    public static DateFormat GetDateFormat(string dateFormat)
    {
        var mapping =
            DateFormats.FirstOrDefault(kv => kv.Value.Equals(dateFormat, StringComparison.OrdinalIgnoreCase));
        if (ValidationHelper.IsNotNull<DateFormat>(mapping.Key) && ValidationHelper.IsNotNull(mapping.Value))
            return mapping.Key;

        throw new ArgumentOutOfRangeException(nameof(dateFormat), "formato de  fecha no soportado.");
    }
}