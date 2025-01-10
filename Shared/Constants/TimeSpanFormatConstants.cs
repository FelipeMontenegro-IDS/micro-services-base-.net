namespace Shared.Constants;

/// <summary>
/// Proporciona constantes para los formatos de <see cref="TimeSpan"/> más comunes.
/// </summary>
/// <remarks>
/// Esta clase contiene constantes que representan diferentes formatos de tiempo para objetos <see cref="TimeSpan"/>.
/// Estas constantes pueden ser utilizadas para formatear intervalos de tiempo de manera consistente,
/// asegurando que se utilicen los valores correctos y evitando errores tipográficos.
/// </remarks>
public static class TimeSpanFormatConstants
{
    /// <summary>
    /// Representa el formato estándar de tiempo (hh:mm:ss).
    /// </summary>
    public const string Standard = @"hh\:mm\:ss";
    
    /// <summary>
    /// Representa el formato corto de tiempo (h:mm).
    /// </summary>
    public const string Short = @"h\:mm";
    
    /// <summary>
    /// Representa el formato largo de tiempo (hh:mm:ss).
    /// </summary>
    public const string Long = @"hh\:mm\:ss";
    
    /// <summary>
    /// Representa el formato de tiempo que incluye días (d.hh:mm:ss).
    /// </summary>
    public const string WithDays = @"d\.hh\:mm\:ss";
    
    /// <summary>
    /// Representa el formato de tiempo que solo incluye minutos y segundos (mm:ss).
    /// </summary>
    public const string MinutesSeconds = @"mm\:ss";
}