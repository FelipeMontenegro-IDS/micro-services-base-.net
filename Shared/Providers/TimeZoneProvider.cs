using Shared.Bases.LookupProvider;
using Shared.Constants;
using Shared.Enums;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers;

namespace Shared.Providers;

public class TimeZoneProvider : BaseLookupProvider<TimeZoneOption, string>, ITimeZoneProvider
{
    public TimeZoneProvider(IValidationHelper validationHelper) : base(new()
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
    },validationHelper)
    {
    }

    public TimeZoneInfo GetTimeZoneInfo(TimeZoneOption timeZoneOption)
    {
        string timeZoneId = GetValue(timeZoneOption, TimeZoneConstants.Utc);
        return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
    }
}