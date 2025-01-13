using Shared.Bases.LookupProvider;
using Shared.Constants;
using Shared.Enums;
using Shared.Helpers;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers;

namespace Shared.Providers;

public class DateFormatProvider : BaseLookupProvider<DateFormat, string>, IDateFormatProvider
{
    public DateFormatProvider(IValidationHelper validationHelper) : base(
        new Dictionary<DateFormat, string>
        {
            { DateFormat.LongDate, DateFormatConstants.LongDate },
            { DateFormat.LongTime, DateFormatConstants.LongTime },
            { DateFormat.ShortDate, DateFormatConstants.ShortDate },
            { DateFormat.ShortTime, DateFormatConstants.ShortTime },
            { DateFormat.IsoDate, DateFormatConstants.IsoDate },
            { DateFormat.FullDateTime, DateFormatConstants.FullDateTime }
        },validationHelper)
    {
    }
}