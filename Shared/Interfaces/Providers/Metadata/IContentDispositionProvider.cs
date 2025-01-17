using Shared.Enums.Metadata;

namespace Shared.Interfaces.Providers.Metadata;

public interface IContentDispositionProvider : Lookup.ILookupProvider<ContentDisposition,string>
{
    string GetValueContentDisposition(ContentDisposition contentDisposition,string fileName);
}