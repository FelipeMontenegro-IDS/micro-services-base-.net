using Shared.Constants;
using Shared.Enums;
using Shared.Helpers;

namespace Shared.Providers;

public static class ContentTypeProvider
{
    private static readonly Dictionary<ContentType, string> ContentTypes = new()
    {
        { ContentType.ApplicationJson, ContentTypeConstants.Json },
        { ContentType.ApplicationXml, ContentTypeConstants.Xml },
        { ContentType.TextPlain, ContentTypeConstants.TextPlain },
        { ContentType.TextHtml, ContentTypeConstants.Html },
        { ContentType.MultipartFormData, ContentTypeConstants.MultipartFormData },
        { ContentType.ApplicationXWwwFormUrlencoded, ContentTypeConstants.FormUrlEncoded },
        { ContentType.ApplicationOctetStream, ContentTypeConstants.OctetStream }
    };

    public static string GetContentType(ContentType contentType)
    {
        if (ContentTypes.TryGetValue(contentType, out var getContentType))
            return getContentType;

        throw new ArgumentOutOfRangeException(nameof(contentType), "Tipo de contenido no soportado.");
    }

    public static ContentType GetContentType(string contentType)
    {
        var mapping =
            ContentTypes.FirstOrDefault(kv => kv.Value.Equals(contentType, StringComparison.OrdinalIgnoreCase));
        if (ValidationHelper.IsNotNull<ContentType>(mapping.Key) && ValidationHelper.IsNotNull(mapping.Value))
            return mapping.Key;

        throw new ArgumentOutOfRangeException(nameof(contentType), "Tipo de contenido no soportado.");
    }

    public static IEnumerable<string> GetAllContentTypes()
    {
        return ContentTypes.Values;
    }
}