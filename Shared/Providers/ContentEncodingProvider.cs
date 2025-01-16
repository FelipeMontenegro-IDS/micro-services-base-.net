using Shared.Bases.Lookup;
using Shared.Constants;
using Shared.Enums;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers;

namespace Shared.Providers;

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