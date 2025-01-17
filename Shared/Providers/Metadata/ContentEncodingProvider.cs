using Shared.Bases.Lookup;
using Shared.Constants.Metadata;
using Shared.Enums.Metadata;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers.Metadata;

namespace Shared.Providers.Metadata;

public class ContentEncodingProvider : BaseLookupProvider<ContentEncoding, string>, IContentEncodingProvider
{
    public ContentEncodingProvider(IValidationHelper validationHelper) : base(new Dictionary<ContentEncoding, string>
    {
        { ContentEncoding.Identity, ContentEncodingConstant.Identity },
        { ContentEncoding.GZip, ContentEncodingConstant.GZip },
        { ContentEncoding.Br, ContentEncodingConstant.Br },
        { ContentEncoding.Compress, ContentEncodingConstant.Compress },
        { ContentEncoding.Deflate, ContentEncodingConstant.Deflate }
    }, validationHelper)
    {
        
    }
}