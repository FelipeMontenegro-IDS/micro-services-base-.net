namespace Shared.Enums.Metadata;

/// <summary>
/// Representa los tipos de codificación de contenido que pueden ser utilizados en encabezados HTTP.
/// </summary>
/// <remarks>
/// Este enum define una serie de valores que especifican cómo se ha codificado el contenido
/// por parte del servidor web. Estas codificaciones son importantes para que el cliente sepa
/// cómo descomprimir o decodificar el contenido recibido.
/// </remarks>
public enum ContentEncoding
{
    /// <summary>
    /// Indica que el contenido ha sido comprimido utilizando el algoritmo GZip.
    /// </summary>
    GZip,

    /// <summary>
    /// Indica que el contenido ha sido comprimido utilizando el algoritmo Compress.
    /// </summary>
    Compress,

    /// <summary>
    /// Indica que el contenido ha sido comprimido utilizando el algoritmo Deflate.
    /// </summary>
    Deflate,

    /// <summary>
    /// Indica que el contenido ha sido comprimido utilizando el algoritmo Brotli.
    /// </summary>
    Br,

    /// <summary>
    /// Indica que el contenido ha sido comprimido utilizando el algoritmo Zstandard.
    /// </summary>
    Zstd,

    /// <summary>
    /// Indica que el contenido no ha sido comprimido y se envía tal como está.
    /// </summary>
    Identity
}