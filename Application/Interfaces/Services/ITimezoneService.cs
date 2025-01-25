using Shared.Enums.Time;

namespace Application.Interfaces.services;

public interface ITimezoneService
{
    TimeZoneOption GetTimezone();
    void SetTimezone(string timezoneId);
}