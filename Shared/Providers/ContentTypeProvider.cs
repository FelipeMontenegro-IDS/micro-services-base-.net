using Shared.Bases.Lookup;
using Shared.Constants;
using Shared.Enums;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers;

namespace Shared.Providers;

public class ContentTypeProvider : BaseLookupProvider<ContentType, string>, IContentTypeProvider
{
    public ContentTypeProvider(IValidationHelper validationHelper) : base(
        new Dictionary<Enums.ContentType, string>
        {
            { ContentType.ApplicationJson, ContentTypeConstant.Json },
            { ContentType.ApplicationXml, ContentTypeConstant.Xml },
            { ContentType.TextPlain, ContentTypeConstant.TextPlain },
            { ContentType.TextHtml, ContentTypeConstant.Html },
            { ContentType.MultipartFormData, ContentTypeConstant.MultipartFormData },
            { ContentType.ApplicationXWwwFormUrlencoded, ContentTypeConstant.FormUrlEncoded },
            { ContentType.ApplicationOctetStream, ContentTypeConstant.OctetStream },
            { ContentType.ApplicationPdf, ContentTypeConstant.Pdf },
            { ContentType.ApplicationZip, ContentTypeConstant.Zip },
            { ContentType.ApplicationGzip, ContentTypeConstant.GZip },
            { ContentType.Jpg, ContentTypeConstant.Jpg },
            { ContentType.Png, ContentTypeConstant.Png },
            { ContentType.Gif, ContentTypeConstant.Gif },
            { ContentType.Tiff, ContentTypeConstant.Tiff },
            { ContentType.Bmp, ContentTypeConstant.Bmp },
            { ContentType.Excel, ContentTypeConstant.Excel }
        }, validationHelper)
    {
    }
}