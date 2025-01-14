using Shared.Bases.LookupProvider;
using Shared.Constants;
using Shared.Enums;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers;

namespace Shared.Providers;

public class ContentEncodingProvider : BaseLookupProvider<ContentEncoding, string>, IContentEncodingProvider
{
    public ContentEncodingProvider(IValidationHelper validationHelper) : base(new Dictionary<ContentEncoding, string>
    {
        { ContentEncoding.Identity, ContentEncodingConstants.Identity },
        { ContentEncoding.Br, ContentEncodingConstants.Br },
        { ContentEncoding.GZip, ContentEncodingConstants.GZip },
        { ContentEncoding.Compress, ContentEncodingConstants.Compress },
        { ContentEncoding.Deflate, ContentEncodingConstants.Deflate }
    }, validationHelper)
    {
        
    }
}