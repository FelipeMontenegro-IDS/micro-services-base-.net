namespace Shared.Enums;

/// <summary>
/// Representa las unidades de medida para tamaños de archivo.
/// </summary>
/// <remarks>
/// Este enum define una lista de unidades de tamaño de archivo que pueden ser utilizadas
/// para especificar la cantidad de datos en bytes, kilobytes, megabytes y gigabytes.
/// Las unidades de tamaño son importantes para la conversión y la gestión de datos en aplicaciones.
/// </remarks>
public enum FileSizeUnit
{
    /// <summary>
    /// Representa la unidad de medida en bytes.
    /// </summary>
    Bytes,
    
    /// <summary>
    /// Representa la unidad de medida en kilobytes.
    /// </summary>
    Kilobytes,
    
    /// <summary>
    /// Representa la unidad de medida en megabytes.
    /// </summary>
    Megabytes,
    
    /// <summary>
    /// Representa la unidad de medida en gigabytes.
    /// </summary>
    Gigabytes
}