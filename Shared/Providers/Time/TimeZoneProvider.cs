using Shared.Bases.Lookup;
using Shared.Constants.Time;
using Shared.Enums.Time;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Time;

namespace Shared.Providers.Time;

public class TimeZoneProvider : BaseLookupProvider<TimeZoneOption, string>, ITimeZoneProvider
{
    public TimeZoneProvider(IValidationHelper validationHelper) : base(new()
    {
        { TimeZoneOption.AmericaLima, TimeZoneConstant.AmericaLima },
        { TimeZoneOption.AmericaLosAngeles, TimeZoneConstant.AmericaLosAngeles },
        { TimeZoneOption.AmericaNewYork, TimeZoneConstant.AmericaNewYork },
        { TimeZoneOption.AmericaChicago, TimeZoneConstant.AmericaChicago },
        { TimeZoneOption.Utc, TimeZoneConstant.Utc },
        { TimeZoneOption.EuropeLondon, TimeZoneConstant.EuropeLondon },
        { TimeZoneOption.EuropeBerlin, TimeZoneConstant.EuropeBerlin },
        { TimeZoneOption.EuropeMoscow, TimeZoneConstant.EuropeMoscow },
        { TimeZoneOption.AsiaKolkata, TimeZoneConstant.AsiaKolkata },
        { TimeZoneOption.AsiaTokyo, TimeZoneConstant.AsiaTokyo },
        { TimeZoneOption.AustraliaSydney, TimeZoneConstant.AustraliaSydney },
        { TimeZoneOption.AmericaDenver, TimeZoneConstant.AmericaDenver },
        { TimeZoneOption.AmericaHalifax, TimeZoneConstant.AmericaHalifax },
        { TimeZoneOption.AmericaArgentinaBuenosAires, TimeZoneConstant.AmericaArgentinaBuenosAires },
        { TimeZoneOption.AmericaSaoPaulo, TimeZoneConstant.AmericaSaoPaulo },
        { TimeZoneOption.AmericaMexicoCity, TimeZoneConstant.AmericaMexicoCity },
        { TimeZoneOption.AmericaCaracas, TimeZoneConstant.AmericaCaracas },
        { TimeZoneOption.AmericaBogota, TimeZoneConstant.AmericaBogota },
        { TimeZoneOption.AmericaMontevideo, TimeZoneConstant.AmericaMontevideo },
        { TimeZoneOption.AmericaGuayaquil, TimeZoneConstant.AmericaGuayaquil },
        { TimeZoneOption.AmericaPanama, TimeZoneConstant.AmericaPanama },
        { TimeZoneOption.AmericaToronto, TimeZoneConstant.AmericaToronto },
        { TimeZoneOption.EuropeMadrid, TimeZoneConstant.EuropeMadrid },
        { TimeZoneOption.EuropeParis, TimeZoneConstant.EuropeParis },
        { TimeZoneOption.EuropeAmsterdam, TimeZoneConstant.EuropeAmsterdam },
        { TimeZoneOption.EuropeRome, TimeZoneConstant.EuropeRome },
        { TimeZoneOption.EuropeZurich, TimeZoneConstant.EuropeZurich },
        { TimeZoneOption.EuropeStockholm, TimeZoneConstant.EuropeStockholm },
        { TimeZoneOption.AsiaSeoul, TimeZoneConstant.AsiaSeoul },
        { TimeZoneOption.AsiaShanghai, TimeZoneConstant.AsiaShanghai },
        { TimeZoneOption.AsiaSingapore, TimeZoneConstant.AsiaSingapore },
        { TimeZoneOption.AsiaManila, TimeZoneConstant.AsiaManila },
        { TimeZoneOption.AsiaDubai, TimeZoneConstant.AsiaDubai },
        { TimeZoneOption.AsiaHongKong, TimeZoneConstant.AsiaHongKong },
        { TimeZoneOption.AsiaKualaLumpur, TimeZoneConstant.AsiaKualaLumpur },
        { TimeZoneOption.AsiaJakarta, TimeZoneConstant.AsiaJakarta },
        { TimeZoneOption.AustraliaBrisbane, TimeZoneConstant.AustraliaBrisbane },
        { TimeZoneOption.AustraliaMelbourne, TimeZoneConstant.AustraliaMelbourne },
        { TimeZoneOption.AustraliaPerth, TimeZoneConstant.AustraliaPerth },
        { TimeZoneOption.PacificAuckland, TimeZoneConstant.PacificAuckland },
        { TimeZoneOption.PacificFiji, TimeZoneConstant.PacificFiji }
    },validationHelper)
    {
    }

    public TimeZoneInfo GetTimeZoneInfo(TimeZoneOption timeZoneOption)
    {
        string timeZoneId = GetValue(timeZoneOption, TimeZoneConstant.Utc);
        return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
    }
}