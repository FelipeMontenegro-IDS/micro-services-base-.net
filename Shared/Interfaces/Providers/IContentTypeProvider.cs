using Shared.Enums;
using Shared.Interfaces.LookupProvider;

namespace Shared.Interfaces.Providers;

public interface IContentTypeProvider : ILookupProvider<ContentType, string> 
{
    
}