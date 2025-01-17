namespace Shared.Enums.Metadata;

/// <summary>
/// Representa los tipos de disposición de contenido utilizados en las respuestas HTTP.
/// </summary>
/// <remarks>
/// Este enum define una lista de disposiciones de contenido que pueden ser utilizadas para especificar
/// cómo se debe manejar el contenido por parte del cliente (por ejemplo, un navegador web).
/// Los tipos de disposición de contenido son importantes para determinar si el contenido debe ser mostrado
/// en línea o descargado como un archivo adjunto.
/// </remarks>
public enum ContentDisposition
{
    /// <summary>
    /// Indica que el contenido debe ser mostrado directamente en el navegador o cliente.
    /// </summary>
    Inline,
    
    /// <summary>
    /// Indica que el contenido debe ser tratado como un archivo adjunto y forzar su descarga.
    /// </summary>
    Attachment
}