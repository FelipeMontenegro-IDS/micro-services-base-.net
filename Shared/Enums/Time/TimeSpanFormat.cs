namespace Shared.Enums.Time;

/// <summary>
/// Representa los formatos de visualización para un objeto TimeSpan.
/// </summary>
/// <remarks>
/// Este enum define una lista de formatos que pueden ser utilizados para representar
/// un intervalo de tiempo (TimeSpan) en diferentes estilos. Los formatos son útiles para
/// mostrar la duración de manera clara y comprensible en aplicaciones.
/// </remarks>
public enum TimeSpanFormat
{
    
    /// <summary>
    /// Formato estándar: hh:mm:ss.
    /// Ejemplo: 01:30:45
    /// </summary>
    Standard,
    
    /// <summary>
    /// Formato corto: h:mm.
    /// Ejemplo: 1:30
    /// </summary>
    Short,
    
    /// <summary>
    /// Formato largo: hh\:mm\:ss.
    /// Ejemplo: 01:30:45
    /// </summary>
    Long,
    
    /// <summary>
    /// Formato que incluye días: d\.hh\:mm\:ss.
    /// Ejemplo: 1.01:30:45
    /// </summary>
    WithDays,
    
    /// <summary>
    /// Formato de minutos y segundos: mm\:ss.
    /// Ejemplo: 90:45
    /// </summary>
    MinutesSeconds 
}