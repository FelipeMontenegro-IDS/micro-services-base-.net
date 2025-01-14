using Shared.Enums;
using Shared.Interfaces.LookupProvider;

namespace Shared.Interfaces.Providers;

public interface IContentDispositionProvider : ILookupProvider<ContentDisposition,string>
{
    string GetValueContentDisposition(ContentDisposition contentDisposition,string fileName);
}