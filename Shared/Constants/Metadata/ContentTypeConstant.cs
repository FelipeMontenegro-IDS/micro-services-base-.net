namespace Shared.Constants.Metadata;

/// <summary>
/// Proporciona constantes para los tipos de contenido MIME más comunes.
/// </summary>
/// <remarks>
/// Esta clase contiene constantes que representan los tipos de contenido utilizados en las solicitudes y respuestas HTTP.
/// Estas constantes pueden ser utilizadas para establecer encabezados de contenido en solicitudes HTTP,
/// asegurando que se utilicen los valores correctos y evitando errores tipográficos.
/// </remarks>
public static class ContentTypeConstant
{
    /// <summary>
    /// Representa el tipo de contenido para JSON.
    /// </summary>
    public const string Json = "application/json";

    /// <summary>
    /// Representa el tipo de contenido para XML.
    /// </summary>
    public const string Xml = "application/xml";

    /// <summary>
    /// Representa el tipo de contenido para texto plano.
    /// </summary>
    public const string TextPlain = "text/plain";

    /// <summary>
    /// Representa el tipo de contenido para HTML.
    /// </summary>
    public const string Html = "text/html";

    /// <summary>
    /// Representa el tipo de contenido para datos de formulario codificados en URL.
    /// </summary>
    public const string FormUrlEncoded = "application/x-www-form-urlencoded";

    /// <summary>
    /// Representa el tipo de contenido para datos de formulario multipart.
    /// </summary>
    public const string MultipartFormData = "multipart/form-data";

    /// <summary>
    /// Representa el tipo de contenido para un flujo de octetos.
    /// </summary>
    public const string OctetStream = "application/octet-stream";
    
    /// <summary>
    /// Representa el tipo de contenido para archivos PDF.
    /// </summary>
    public const string Pdf = "application/pdf";
    
    /// <summary>
    /// Representa el tipo de contenido para archivos ZIP.
    /// </summary>
    public const string Zip = "application/zip";
        
    /// <summary>
    /// Representa el tipo de contenido para archivos GZip.
    /// </summary>
    public const string GZip = "application/gzip";
    
    /// <summary>
    /// Representa el tipo MIME para imágenes JPEG.
    /// </summary>
    public const string Jpg = "image/jpeg";

    /// <summary>
    /// Representa el tipo MIME para imágenes GIF.
    /// </summary>
    public const string Gif = "image/gif";

    /// <summary>
    /// Representa el tipo MIME para imágenes PNG.
    /// </summary>
    public const string Png = "image/png";

    /// <summary>
    /// Representa el tipo MIME para imágenes TIFF.
    /// </summary>
    public const string Tiff = "image/tiff";

    /// <summary>
    /// Representa el tipo MIME para imágenes BMP.
    /// </summary>
    public const string Bmp = "image/bmp";
    
    /// <summary>
    /// Representa el tipo MIME para Excel.
    /// </summary>
    public const string Excel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
}