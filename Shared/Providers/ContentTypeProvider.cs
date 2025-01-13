using Shared.Bases.LookupProvider;
using Shared.Constants;
using Shared.Enums;
using Shared.Helpers;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers;

namespace Shared.Providers;

public class ContentTypeProvider : BaseLookupProvider<ContentType, string>, IContentTypeProvider
{
    public ContentTypeProvider(IValidationHelper validationHelper) : base(
        new Dictionary<ContentType, string>
        {
            { ContentType.ApplicationJson, ContentTypeConstants.Json },
            { ContentType.ApplicationXml, ContentTypeConstants.Xml },
            { ContentType.TextPlain, ContentTypeConstants.TextPlain },
            { ContentType.TextHtml, ContentTypeConstants.Html },
            { ContentType.MultipartFormData, ContentTypeConstants.MultipartFormData },
            { ContentType.ApplicationXWwwFormUrlencoded, ContentTypeConstants.FormUrlEncoded },
            { ContentType.ApplicationOctetStream, ContentTypeConstants.OctetStream },
            { ContentType.ApplicationPdf, ContentTypeConstants.Pdf },
            { ContentType.ApplicationZip, ContentTypeConstants.Zip },
            { ContentType.ApplicationGzip, ContentTypeConstants.GZip },
            { ContentType.Jpg, ContentTypeConstants.Jpg },
            { ContentType.Png, ContentTypeConstants.Png },
            { ContentType.Gif, ContentTypeConstants.Gif },
            { ContentType.Tiff, ContentTypeConstants.Tiff },
            { ContentType.Bmp, ContentTypeConstants.Bmp }
        }, validationHelper)
    {
    }
}