using Shared.Bases.Lookup;
using Shared.Constants;
using Shared.Enums;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers;

namespace Shared.Providers;

public class TimeSpanFormatProvider : BaseLookupProvider<TimeSpanFormat, string>, ITimeSpanFormatProvider
{
    public TimeSpanFormatProvider(IValidationHelper validationHelper) : base(new()
    {
        { TimeSpanFormat.Long, TimeSpanFormatConstant.Long },
        { TimeSpanFormat.Short, TimeSpanFormatConstant.Short },
        { TimeSpanFormat.Standard, TimeSpanFormatConstant.Standard },
        { TimeSpanFormat.WithDays, TimeSpanFormatConstant.WithDays },
        { TimeSpanFormat.MinutesSeconds, TimeSpanFormatConstant.MinutesSeconds }
    },validationHelper)
    {
    }
}