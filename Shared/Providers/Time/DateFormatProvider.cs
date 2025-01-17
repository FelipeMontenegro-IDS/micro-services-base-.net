using Shared.Bases.Lookup;
using Shared.Constants.Time;
using Shared.Enums.Time;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Time;

namespace Shared.Providers.Time;

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