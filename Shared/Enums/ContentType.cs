namespace Shared.Enums;

/// <summary>
/// Representa los tipos de contenido MIME utilizados en las solicitudes y respuestas HTTP.
/// </summary>
/// <remarks>
/// Este enum define una lista de tipos de contenido que pueden ser utilizados para especificar
/// el tipo de datos que se están enviando o recibiendo en una comunicación HTTP.
/// Los tipos de contenido son importantes para que el servidor y el cliente interpreten correctamente
/// los datos transmitidos.
/// </remarks>
public enum ContentType
{
    /// <summary>
    /// Representa el tipo de contenido para datos en formato JSON.
    /// </summary>
    ApplicationJson,
    
    /// <summary>
    /// Representa el tipo de contenido para datos en formato XML.
    /// </summary>
    ApplicationXml,
    
    /// <summary>
    /// Representa el tipo de contenido para texto plano.
    /// </summary>
    TextPlain,
    
    /// <summary>
    /// Representa el tipo de contenido para documentos HTML.
    /// </summary>
    TextHtml,
    
    /// <summary>
    /// Representa el tipo de contenido para formularios de tipo multipart.
    /// </summary>
    MultipartFormData,
    
    /// <summary>
    /// Representa el tipo de contenido para datos de formularios codificados en URL.
    /// </summary>
    ApplicationXWwwFormUrlencoded,
    
    /// <summary>
    /// Representa el tipo de contenido para datos binarios genéricos.
    /// </summary>
    ApplicationOctetStream,
    
    /// <summary>
    /// Representa el tipo de contenido para archivos PDF.
    /// </summary>
    ApplicationPdf
}