namespace Shared.Enums.Metadata;

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
    ApplicationPdf,
    
    /// <summary>
    /// Este tipo MIME se utiliza para archivos comprimidos en formato ZIP. Es comúnmente utilizado para agrupar múltiples archivos en un solo archivo comprimido, facilitando su almacenamiento y transferencia.
    /// </summary>
    ApplicationZip,
    
    /// <summary>
    ///  Este tipo MIME se utiliza para archivos comprimidos en formato GZip. GZip es un formato de compresión que se utiliza frecuentemente para reducir el tamaño de archivos y mejorar la velocidad de transferencia en la web.
    /// </summary>
    ApplicationGzip,
    
    /// <summary>
    /// Este tipo MIME se utiliza para imágenes en formato JPEG, que es un formato popular para fotografías debido a su capacidad de compresión con pérdida, lo que reduce el tamaño del archivo.
    /// </summary>
    Jpg,
    
    /// <summary>
    /// Este tipo MIME se utiliza para imágenes en formato PNG, que es conocido por su compresión sin pérdida y soporte de transparencias, lo que lo hace ideal para gráficos y logotipos.
    /// </summary>
    Png,
    
    /// <summary>
    /// Este tipo MIME se utiliza para imágenes en formato GIF, que es conocido por su capacidad de soportar animaciones y transparencias, aunque tiene una paleta de colores limitada.
    /// </summary>
    Gif,
    
    /// <summary>
    /// Este tipo MIME se utiliza para imágenes en formato TIFF, que es común en la impresión y almacenamiento de imágenes de alta calidad, especialmente en entornos profesionales.
    /// </summary>
    Tiff,
    
    /// <summary>
    /// Este tipo MIME se utiliza para imágenes en formato BMP, que es un formato de imagen sin compresión utilizado principalmente en sistemas Windows.
    /// </summary>
    Bmp,
    
    /// <summary>
    /// Este tipo MIME se utiliza para Excels, que es un formato de hojas de cálculo.
    /// </summary>
    Excel
    
}