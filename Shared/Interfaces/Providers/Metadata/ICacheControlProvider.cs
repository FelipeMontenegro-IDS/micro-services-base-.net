using Shared.Enums.Metadata;
using Shared.Interfaces.Lookup;

namespace Shared.Interfaces.Providers.Metadata;

public interface ICacheControlProvider : ILookupProvider<CacheControl, string> 
{
    string GetValueCacheControl(CacheControl cacheControl);
}