using Shared.Utils.Enums;
using Shared.Utils.Helpers;

namespace Shared.Utils.Providers;

public static class ContentTypeProvider
{
    private static readonly Dictionary<ContentType, string> ContentTypes = new()
    {
        { ContentType.ApplicationJson, "application/json" },
        { ContentType.ApplicationXml, "application/xml" },
        { ContentType.TextPlain, "text/plain" },
        { ContentType.TextHtml, "text/html" },
        { ContentType.MultipartFormData, "multipart/form-data" },
        { ContentType.ApplicationXWwwFormUrlencoded, "application/x-www-form-urlencoded" },
        { ContentType.ApplicationOctetStream, "application/octet-stream" }
    };
    
    public static string GetContentType(ContentType contentType)
    {
        if (ContentTypes.TryGetValue(contentType, out var getContentType))
            return getContentType;

        throw new ArgumentOutOfRangeException(nameof(contentType), "Tipo de contenido no soportado.");
    }
    
    public static ContentType GetContentTypeOption(string contentType)
    {
        var mapping = ContentTypes.FirstOrDefault(kv => kv.Value.Equals(contentType, StringComparison.OrdinalIgnoreCase));
        if (!Equals(mapping.Key, default(ContentType)) || ValidationHelper.IsNotNull( mapping.Value))
            return mapping.Key;
        
        throw new ArgumentOutOfRangeException(nameof(contentType), "Tipo de contenido no soportado.");
    }

    public static IEnumerable<string> GetAllContentTypes()
    {
        return ContentTypes.Values;
    }

}