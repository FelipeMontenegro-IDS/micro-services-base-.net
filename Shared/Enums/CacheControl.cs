namespace Shared.Enums;

/// <summary>
/// Representa las directivas de control de caché que pueden ser utilizadas en encabezados HTTP.
/// </summary>
/// <remarks>
/// Este enum define una serie de valores que especifican cómo se debe manejar la caché de los recursos
/// en las respuestas HTTP. Estas directivas son importantes para optimizar el rendimiento y la seguridad
/// de las aplicaciones web al controlar el almacenamiento y la reutilización de respuestas en caché.
/// </remarks>
public enum CacheControl
{
    /// <summary>
    /// Indica que la respuesta no debe ser almacenada en caché.
    /// </summary>
    NoCache,

    /// <summary>
    /// Indica que la respuesta no debe ser almacenada en caché en absoluto.
    /// </summary>
    NoStore,

    /// <summary>
    /// Indica que la respuesta puede ser almacenada en caché y compartida entre múltiples usuarios.
    /// </summary>
    Public,

    /// <summary>
    /// Indica que la respuesta es específica para un solo usuario y no debe ser almacenada en caché
    /// en un caché compartido.
    /// </summary>
    Private,

    /// <summary>
    /// Indica que la respuesta puede ser almacenada en caché, pero debe expirar después de un tiempo
    /// máximo especificado.
    /// </summary>
    MaxAge,

    /// <summary>
    /// Indica que la respuesta puede ser almacenada en caché, pero solo en cachés compartidos,
    /// y debe expirar después de un tiempo máximo especificado.
    /// </summary>
    SMaxAge
}