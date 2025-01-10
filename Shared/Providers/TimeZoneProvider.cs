using Shared.Constants;
using Shared.Enums;

namespace Shared.Providers;

public static class TimeZoneProvider
{
    private static readonly Dictionary<TimeZoneOption, string> TimeZones = new()
    {
        { TimeZoneOption.AmericaLima, TimeZoneConstants.AmericaLima },
        { TimeZoneOption.AmericaLosAngeles, TimeZoneConstants.AmericaLosAngeles },
        { TimeZoneOption.AmericaNewYork, TimeZoneConstants.AmericaNewYork },
        { TimeZoneOption.AmericaChicago, TimeZoneConstants.AmericaChicago },
        { TimeZoneOption.Utc, TimeZoneConstants.Utc },
        { TimeZoneOption.EuropeLondon, TimeZoneConstants.EuropeLondon },
        { TimeZoneOption.EuropeBerlin, TimeZoneConstants.EuropeBerlin },
        { TimeZoneOption.EuropeMoscow, TimeZoneConstants.EuropeMoscow },
        { TimeZoneOption.AsiaKolkata, TimeZoneConstants.AsiaKolkata },
        { TimeZoneOption.AsiaTokyo, TimeZoneConstants.AsiaTokyo },
        { TimeZoneOption.AustraliaSydney, TimeZoneConstants.AustraliaSydney },
        { TimeZoneOption.AmericaDenver, TimeZoneConstants.AmericaDenver },
        { TimeZoneOption.AmericaHalifax, TimeZoneConstants.AmericaHalifax },
        { TimeZoneOption.AmericaArgentinaBuenosAires, TimeZoneConstants.AmericaArgentinaBuenosAires },
        { TimeZoneOption.AmericaSaoPaulo, TimeZoneConstants.AmericaSaoPaulo },
        { TimeZoneOption.AmericaMexicoCity, TimeZoneConstants.AmericaMexicoCity },
        { TimeZoneOption.AmericaCaracas, TimeZoneConstants.AmericaCaracas },
        { TimeZoneOption.AmericaBogota, TimeZoneConstants.AmericaBogota },
        { TimeZoneOption.AmericaMontevideo, TimeZoneConstants.AmericaMontevideo },
        { TimeZoneOption.AmericaGuayaquil, TimeZoneConstants.AmericaGuayaquil },
        { TimeZoneOption.AmericaPanama, TimeZoneConstants.AmericaPanama },
        { TimeZoneOption.AmericaToronto, TimeZoneConstants.AmericaToronto },
        { TimeZoneOption.EuropeMadrid, TimeZoneConstants.EuropeMadrid },
        { TimeZoneOption.EuropeParis, TimeZoneConstants.EuropeParis },
        { TimeZoneOption.EuropeAmsterdam, TimeZoneConstants.EuropeAmsterdam },
        { TimeZoneOption.EuropeRome, TimeZoneConstants.EuropeRome },
        { TimeZoneOption.EuropeZurich, TimeZoneConstants.EuropeZurich },
        { TimeZoneOption.EuropeStockholm, TimeZoneConstants.EuropeStockholm },
        { TimeZoneOption.AsiaSeoul, TimeZoneConstants.AsiaSeoul },
        { TimeZoneOption.AsiaShanghai, TimeZoneConstants.AsiaShanghai },
        { TimeZoneOption.AsiaSingapore, TimeZoneConstants.AsiaSingapore },
        { TimeZoneOption.AsiaManila, TimeZoneConstants.AsiaManila },
        { TimeZoneOption.AsiaDubai, TimeZoneConstants.AsiaDubai },
        { TimeZoneOption.AsiaHongKong, TimeZoneConstants.AsiaHongKong },
        { TimeZoneOption.AsiaKualaLumpur, TimeZoneConstants.AsiaKualaLumpur },
        { TimeZoneOption.AsiaJakarta, TimeZoneConstants.AsiaJakarta },
        { TimeZoneOption.AustraliaBrisbane, TimeZoneConstants.AustraliaBrisbane },
        { TimeZoneOption.AustraliaMelbourne, TimeZoneConstants.AustraliaMelbourne },
        { TimeZoneOption.AustraliaPerth, TimeZoneConstants.AustraliaPerth },
        { TimeZoneOption.PacificAuckland, TimeZoneConstants.PacificAuckland },
        { TimeZoneOption.PacificFiji, TimeZoneConstants.PacificFiji }
    };

    public static string GetTimeZone(TimeZoneOption timeZoneOption)
    {
        return TimeZones.GetValueOrDefault(timeZoneOption, TimeZoneConstants.Utc);
    }

    public static TimeZoneOption GetTimeZone(string timeZone)
    {
        var mapping = TimeZones.FirstOrDefault(kv => kv.Value == timeZone);
        if (mapping.Key != default) return mapping.Key;
        return TimeZoneOption.Utc;
    }

    public static TimeZoneInfo GetTimeZoneInfo(TimeZoneOption timeZoneOption)
    {
        var timeZoneId = GetTimeZone(timeZoneOption);
        return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
    }
    
    public static IEnumerable<string> GetAllTimeZoneInfos()
    {
        return TimeZones.Values;
    }
}