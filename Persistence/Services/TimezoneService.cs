using Application.Interfaces.services;
using Microsoft.AspNetCore.Http;
using Shared.Enums.Time;
using Shared.Interfaces.Providers.Time;

namespace Persistence.Services;

public class TimezoneService : ITimezoneService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITimeZoneProvider _timeZoneProvider;


    public TimezoneService(ITimeZoneProvider timeZoneProvider, IHttpContextAccessor httpContextAccessor)
    {
        _timeZoneProvider = timeZoneProvider;
        _httpContextAccessor = httpContextAccessor;
    }

    public TimeZoneOption GetTimezone()
    {
        string? timezoneId = _httpContextAccessor.HttpContext.Items["Timezone"] as string;

        if (string.IsNullOrEmpty(timezoneId))
            throw new ArgumentException("Timezone not found.");
        
        if (!IsTimezoneSupported(timezoneId))
            throw new ArgumentException($"Timezone '{timezoneId}' is not supported.");
        
        return _timeZoneProvider.GetKey(timezoneId);
    }

    public void SetTimezone(string timezoneId)
    {
        if (!IsTimezoneSupported(timezoneId))
            throw new ArgumentException($"Timezone '{timezoneId}' is not supported.");

        _httpContextAccessor.HttpContext.Items["Timezone"] = timezoneId;
    }
    
    private bool IsTimezoneSupported(string timezoneId)
    {
        return _timeZoneProvider.ContainsValue(timezoneId);
    }
}