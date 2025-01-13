namespace Shared.Constants;

/// <summary>
/// Proporciona constantes para los tipos de disposición de contenido utilizados en las respuestas HTTP.
/// </summary>
/// <remarks>
/// Esta clase contiene constantes que representan los valores de disposición de contenido que pueden ser utilizados
/// para especificar cómo se debe manejar el contenido por parte del cliente (por ejemplo, un navegador web).
/// Los tipos de disposición de contenido son importantes para determinar si el contenido debe ser mostrado en línea
/// o descargado como un archivo adjunto.
/// </remarks>
public static class ContentDispositionConstants
{
    /// <summary>
    /// Representa la disposición de contenido para mostrar el contenido en línea.
    /// </summary>
    public const string Inline = "inline";
    
    /// <summary>
    /// Representa la disposición de contenido para forzar la descarga del contenido como un archivo adjunto.
    /// </summary>
    public const string Attachment = "attachment";
}