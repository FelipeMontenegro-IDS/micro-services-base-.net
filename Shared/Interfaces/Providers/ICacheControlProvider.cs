using Shared.Enums;
using Shared.Interfaces.Lookup;

namespace Shared.Interfaces.Providers;

public interface ICacheControlProvider : ILookupProvider<CacheControl, string> 
{
    string GetValueCacheControl(CacheControl cacheControl);
}