using Shared.Bases.LookupProvider;
using Shared.Constants;
using Shared.Converters;
using Shared.Enums;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers;

namespace Shared.Providers;

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
            { CacheControl.NoCache, CacheControlConstants.NoCache },
            { CacheControl.NoStore, CacheControlConstants.NoStore },
            { CacheControl.Private, CacheControlConstants.Private },
            { CacheControl.Public, CacheControlConstants.Public },
            { CacheControl.MaxAge, CacheControlConstants.MaxAge },
            { CacheControl.SMaxAge, CacheControlConstants.SMaxAge }
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
        string value = GetValue(cacheControl, CacheControlConstants.NoCache);

        return value switch
        {
            CacheControlConstants.SMaxAge =>
                $"{CacheControlConstants.SMaxAge}{(int)_timeConverter.Convert(24, TimeUnit.Hours, TimeUnit.Seconds)}",
            CacheControlConstants.MaxAge =>
                $"{CacheControlConstants.MaxAge}{_timeConverter.Convert(1, TimeUnit.Hours, TimeUnit.Seconds)}",
            _ => value
        };
    }
}