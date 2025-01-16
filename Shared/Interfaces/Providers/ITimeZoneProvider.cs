using Shared.Enums;

namespace Shared.Interfaces.Providers;

public interface ITimeZoneProvider : Lookup.ILookupProvider<TimeZoneOption, string>
{
    TimeZoneInfo GetTimeZoneInfo(TimeZoneOption timeZoneOption);
}