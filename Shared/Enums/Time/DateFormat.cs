namespace Shared.Enums.Time;

/// <summary>
/// Representa los formatos de fecha utilizados en la aplicación.
/// </summary>
/// <remarks>
/// Este enum define una lista de formatos de fecha que pueden ser utilizados para
/// mostrar fechas en diferentes estilos. Los formatos de fecha son importantes para
/// garantizar que las fechas se presenten de manera clara y comprensible para los usuarios.
/// </remarks>
public enum DateFormat
{
    /// <summary>
    /// Representa un formato de fecha corto (ejemplo: 01/01/2025).
    /// </summary>
    ShortDate,
    
    /// <summary>
    /// Representa un formato de fecha largo (ejemplo: 1 de enero de 2025).
    /// </summary>
    LongDate,
    
    /// <summary>
    /// Representa un formato de fecha y hora completo (ejemplo: 1 de enero de 2025 12:00 PM).
    /// </summary>
    FullDateTime,
    
    /// <summary>
    /// Representa un formato de hora corto (ejemplo: 12:00 PM).
    /// </summary>
    ShortTime,
    
    /// <summary>
    /// Representa un formato de hora largo (ejemplo: 12:00:00 PM).
    /// </summary>
    LongTime, 
    
    /// <summary>
    /// Representa un formato de fecha ISO (ejemplo: 2025-01-01).
    /// </summary>
    IsoDate
}