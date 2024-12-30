using Shared.Utils.Enums;

namespace Shared.Utils.Collections;

public static class CollectionTimeZone
{
    private static readonly Dictionary<TimeZoneOption, string> TimeZones = new()
    {
        { TimeZoneOption.AmericaLima, "America/Lima" },
        { TimeZoneOption.AmericaLosAngeles, "America/Los_Angeles" },
        { TimeZoneOption.AmericaNewYork, "America/New_York" },
        { TimeZoneOption.AmericaChicago, "America/Chicago" },
        { TimeZoneOption.Utc, "UTC" },
        { TimeZoneOption.EuropeLondon, "Europe/London" },
        { TimeZoneOption.EuropeBerlin, "Europe/Berlin" },
        { TimeZoneOption.EuropeMoscow, "Europe/Moscow" },
        { TimeZoneOption.AsiaKolkata, "Asia/Kolkata" },
        { TimeZoneOption.AsiaTokyo, "Asia/Tokyo" },
        { TimeZoneOption.AustraliaSydney, "Australia/Sydney" },
        { TimeZoneOption.AmericaDenver, "America/Denver" },
        { TimeZoneOption.AmericaHalifax, "America/Halifax" },
        { TimeZoneOption.AmericaArgentinaBuenosAires, "America/Argentina/Buenos_Aires" },
        { TimeZoneOption.AmericaSaoPaulo, "America/Sao_Paulo" },
        { TimeZoneOption.AmericaMexicoCity, "America/Mexico_City" },
        { TimeZoneOption.AmericaCaracas, "America/Caracas" },
        { TimeZoneOption.AmericaBogota, "America/Bogota" },
        { TimeZoneOption.AmericaMontevideo, "America/Montevideo" },
        { TimeZoneOption.AmericaGuayaquil, "America/Guayaquil" },
        { TimeZoneOption.AmericaPanama, "America/Panama" },
        { TimeZoneOption.AmericaToronto, "America/Toronto" },
        { TimeZoneOption.EuropeMadrid, "Europe/Madrid" },
        { TimeZoneOption.EuropeParis, "Europe/Paris" },
        { TimeZoneOption.EuropeAmsterdam, "Europe/Amsterdam" },
        { TimeZoneOption.EuropeRome, "Europe/Rome" },
        { TimeZoneOption.EuropeZurich, "Europe/Zurich" },
        { TimeZoneOption.EuropeStockholm, "Europe/Stockholm" },
        { TimeZoneOption.AsiaSeoul, "Asia/Seoul" },
        { TimeZoneOption.AsiaShanghai, "Asia/Shanghai" },
        { TimeZoneOption.AsiaSingapore, "Asia/Singapore" },
        { TimeZoneOption.AsiaManila, "Asia/Manila" },
        { TimeZoneOption.AsiaDubai, "Asia/Dubai" },
        { TimeZoneOption.AsiaHongKong, "Asia/Hong Kong" },
        { TimeZoneOption.AsiaKualaLumpur, "Asia/Kuala Lumpur" },
        { TimeZoneOption.AsiaJakarta, "Asia/Jakarta" },
        { TimeZoneOption.AustraliaBrisbane, "Australia/Brisbane" },
        { TimeZoneOption.AustraliaMelbourne, "Australia/Melbourne" },
        { TimeZoneOption.AustraliaPerth, "Australia/Perth" },
        { TimeZoneOption.PacificAuckland, "Pacific/Auckland" },
        { TimeZoneOption.PacificFiji, "Pacific/Fiji" }
    };
    
    public static string GetTimeZoneOption(TimeZoneOption timeZoneOption)
    {
        if (TimeZones.TryGetValue(timeZoneOption, out var timeZoneId))
            return timeZoneId;
        
        throw new ArgumentOutOfRangeException(nameof(timeZoneOption), "Zona horaria no soportada.");
    }
    
    public static TimeZoneOption GetTimeZoneOption(string timeZone)
    {
        var mapping = TimeZones.FirstOrDefault(kv => kv.Value == timeZone);
        if (mapping.Key != default)
            return mapping.Key;
        
        throw new ArgumentOutOfRangeException(nameof(timeZone), "Zona horaria no soportada.");
    }
    
    public static TimeZoneInfo GetTimeZoneInfo(TimeZoneOption timeZoneOption)
    {
        var timeZoneId = GetTimeZoneOption(timeZoneOption);
        return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
    }
}