namespace Shared.Constants;

/// <summary>
/// Proporciona constantes para los formatos de fecha y hora más comunes.
/// </summary>
/// <remarks>
/// Esta clase contiene constantes que representan los formatos de fecha y hora utilizados en diversas aplicaciones.
/// Estas constantes pueden ser utilizadas para formatear fechas y horas de manera consistente,
/// asegurando que se utilicen los valores correctos y evitando errores tipográficos.
/// </remarks>
public static class DateFormatConstants
{
    /// <summary>
    /// Representa el formato corto de fecha (MM/dd/yyyy).
    /// </summary>
    public const string ShortDate = "MM/dd/yyyy";                       
    
    /// <summary>
    /// Representa el formato largo de fecha (dddd, MMMM dd, yyyy).
    /// </summary>
    public const string LongDate = "dddd, MMMM dd, yyyy";               
    
    /// <summary>
    /// Representa el formato completo de fecha y hora (f).
    /// </summary>
    public const string FullDateTime = "f";                             
    
    /// <summary>
    /// Representa el formato de hora corta (hh:mm tt).
    /// </summary>
    public const string ShortTime = "hh:mm tt";                         
    
    /// <summary>
    /// Representa el formato de hora larga (HH:mm:ss).
    /// </summary>
    public const string LongTime = "HH:mm:ss";                          
    
    /// <summary>
    /// Representa el formato de fecha ISO (yyyy-MM-dd).
    /// </summary>
    public const string IsoDate = "yyyy-MM-dd";            
}