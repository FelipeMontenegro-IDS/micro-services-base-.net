using Shared.Bases.Lookup;
using Shared.Constants;
using Shared.Enums;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers;

namespace Shared.Providers;

public class DateFormatProvider : BaseLookupProvider<DateFormat, string>, IDateFormatProvider
{
    public DateFormatProvider(IValidationHelper validationHelper) : base(
        new Dictionary<DateFormat, string>
        {
            { DateFormat.LongDate, DateFormatConstant.LongDate },
            { DateFormat.LongTime, DateFormatConstant.LongTime },
            { DateFormat.ShortDate, DateFormatConstant.ShortDate },
            { DateFormat.ShortTime, DateFormatConstant.ShortTime },
            { DateFormat.IsoDate, DateFormatConstant.IsoDate },
            { DateFormat.FullDateTime, DateFormatConstant.FullDateTime }
        },validationHelper)
    {
    }
}