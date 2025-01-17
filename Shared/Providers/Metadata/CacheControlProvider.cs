using Shared.Bases.Lookup;
using Shared.Constants.Metadata;
using Shared.Converters;
using Shared.Enums.Metadata;
using Shared.Enums.Time;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Metadata;

namespace Shared.Providers.Metadata;

/// <summary>
/// Proveedor de control de caché que extiende la funcionalidad de BaseLookupProvider.
/// Esta clase se encarga de gestionar las directivas de control de caché HTTP.
/// </summary>
public class CacheControlProvider : BaseLookupProvider<CacheControl, string>, ICacheControlProvider
{
    private readonly TimeConverter _timeConverter;

    public CacheControlProvider(IValidationHelper validationHelper, TimeConverter timeConverter) : base(
        new Dictionary<CacheControl, string>
        {
            { CacheControl.NoCache, CacheControlConstant.NoCache },
            { CacheControl.NoStore, CacheControlConstant.NoStore },
            { CacheControl.Private, CacheControlConstant.Private },
            { CacheControl.Public, CacheControlConstant.Public },
            { CacheControl.MaxAge, CacheControlConstant.MaxAge },
            { CacheControl.SMaxAge, CacheControlConstant.SMaxAge }
        }, validationHelper)
    {
        _timeConverter = timeConverter;
    }

    /// <summary>
    /// Obtiene el valor de control de caché correspondiente a la directiva especificada.
    /// </summary>
    /// <param name="cacheControl">La directiva de control de caché a obtener.</param>
    /// <returns>El valor de control de caché como una cadena.</returns>
    public string GetValueCacheControl(CacheControl cacheControl)
    {
        string value = GetValue(cacheControl, CacheControlConstant.NoCache);

        return value switch
        {
            CacheControlConstant.SMaxAge =>
                $"{CacheControlConstant.SMaxAge}{(int)_timeConverter.Convert(24, TimeUnit.Hours, TimeUnit.Seconds)}",
            CacheControlConstant.MaxAge =>
                $"{CacheControlConstant.MaxAge}{_timeConverter.Convert(1, TimeUnit.Hours, TimeUnit.Seconds)}",
            _ => value
        };
    }
}