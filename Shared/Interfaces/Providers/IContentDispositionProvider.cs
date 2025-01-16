using Shared.Enums;

namespace Shared.Interfaces.Providers;

public interface IContentDispositionProvider : Lookup.ILookupProvider<ContentDisposition,string>
{
    string GetValueContentDisposition(ContentDisposition contentDisposition,string fileName);
}