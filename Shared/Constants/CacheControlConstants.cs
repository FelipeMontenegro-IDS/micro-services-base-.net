namespace Shared.Constants;

/// <summary>
/// Clase estática que contiene constantes para el control de caché HTTP.
/// Estas constantes se utilizan para definir directivas de caché en las respuestas HTTP.
/// </summary>
public static  class CacheControlConstants
{
    /// <summary>
    /// Directiva que indica que el recurso no debe ser almacenado en caché.
    /// </summary>
    public const string NoCache = "no-cache";
    
    /// <summary>
    /// Directiva que indica que el recurso no debe ser almacenado en absoluto.
    /// </summary>
    public const string NoStore = "no-store";
    
    /// <summary>
    /// Directiva que permite que el recurso sea almacenado en cachés compartidos.
    /// </summary>
    public const string Public = "public";
    
    /// <summary>
    /// Directiva que indica que el recurso es privado y no debe ser almacenado en cachés compartidos.
    /// </summary>
    public const string Private = "private";

    /// <summary segundos="&quot;.">
    /// Directiva que especifica el tiempo máximo que un recurso puede ser almacenado en caché, en segundos.
    /// Se utiliza en la forma "max-age=segundos
    /// </summary>
    public const string MaxAge = "max-age=";
    
    /// <summary>
    /// Directiva que especifica el tiempo máximo que un recurso puede ser almacenado en cachés compartidos, en segundos.
    /// Se utiliza en la forma "s-maxage=segundos".
    /// </summary>
    public const string SMaxAge = "s-maxage=";

}