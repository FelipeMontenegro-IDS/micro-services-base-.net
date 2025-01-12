using Shared.Bases.LookupProvider;
using Shared.Constants;
using Shared.Enums;
using Shared.Helpers;
using Shared.Interfaces.Providers;

namespace Shared.Providers;

public class TimeSpanFormatProvider : BaseLookupProvider<TimeSpanFormat, string>, ITimeSpanFormatProvider
{
    protected TimeSpanFormatProvider() : base(new()
    {
        { TimeSpanFormat.Long, TimeSpanFormatConstants.Long },
        { TimeSpanFormat.Short, TimeSpanFormatConstants.Short },
        { TimeSpanFormat.Standard, TimeSpanFormatConstants.Standard },
        { TimeSpanFormat.WithDays, TimeSpanFormatConstants.WithDays },
        { TimeSpanFormat.MinutesSeconds, TimeSpanFormatConstants.MinutesSeconds }
    })
    {
    }
}