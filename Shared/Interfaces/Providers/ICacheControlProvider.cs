using Shared.Enums;
using Shared.Interfaces.LookupProvider;

namespace Shared.Interfaces.Providers;

public interface ICacheControlProvider :  ILookupProvider<CacheControl, string> 
{
    string GetValueCacheControl(CacheControl cacheControl);
}