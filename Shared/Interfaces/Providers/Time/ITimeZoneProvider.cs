using Shared.Enums.Time;

namespace Shared.Interfaces.Providers.Time;

public interface ITimeZoneProvider : Lookup.ILookupProvider<TimeZoneOption, string>
{
    TimeZoneInfo GetTimeZoneInfo(TimeZoneOption timeZoneOption);
}