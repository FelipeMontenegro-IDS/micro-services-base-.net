using Shared.Enums;

namespace Shared.Converters;

/// <summary>
/// Clase de ayuda para convertir tamaños de archivos entre diferentes unidades.
/// </summary>
public  class FileSizeConverter
{
    /// <summary>
    /// Representa el número de bytes en un kilobyte.
    /// </summary>
    private const long BytesInKilobyte = 1024;
    
    /// <summary>
    /// Representa el número de bytes en un megabyte.
    /// </summary>
    private const long BytesInMegabyte = BytesInKilobyte * 1024;
    
    /// <summary>
    /// Representa el número de bytes en un gigabyte.
    /// </summary>
    private const long BytesInGigabyte = BytesInMegabyte * 1024;


    
    /// <summary>
    /// Convierte un tamaño de archivo de una unidad a otra.
    /// </summary>
    /// <param name="size">El tamaño a convertir.</param>
    /// <param name="fromUnit">La unidad de origen del tamaño.</param>
    /// <param name="toUnit">La unidad de destino a la que se convertirá el tamaño.</param>
    /// <returns>El tamaño convertido en la unidad de destino.</returns>
    /// <exception cref="NotSupportedException">Se lanza si se proporciona una unidad no soportada.</exception>
    public long Convert(long size, FileSizeUnit fromUnit, FileSizeUnit toUnit)
    {
        long sizeInBytes = fromUnit switch
        {
            FileSizeUnit.Kilobytes => KilobytesToBytes(size),
            FileSizeUnit.Megabytes => MegabytesToBytes(size),
            FileSizeUnit.Gigabytes => GigabytesToBytes(size),
            FileSizeUnit.Bytes => size,
            _ => throw new NotSupportedException("Unknown FileSizeUnit")
        };

        return toUnit switch
        {
            FileSizeUnit.Kilobytes => BytesToKilobytes(sizeInBytes),
            FileSizeUnit.Megabytes => BytesToMegabytes(sizeInBytes),
            FileSizeUnit.Gigabytes => BytesToGigabytes(sizeInBytes),
            FileSizeUnit.Bytes => sizeInBytes,
            _ => throw new NotSupportedException("Unknown FileSizeUnit")
        };
    }

    /// <summary>
    /// Convierte un tamaño en bytes a kilobytes.
    /// </summary>
    /// <param name="bytes">El tamaño en bytes a convertir.</param>
    /// <returns>El tamaño en kilobytes.</returns>
    private long BytesToKilobytes(long bytes)
    {
        return bytes / BytesInKilobyte;
    }


    /// <summary>
    /// Convierte un tamaño en bytes a megabytes.
    /// </summary>
    /// <param name="bytes">El tamaño en bytes a convertir.</param>
    /// <returns>El tamaño en megabytes.</returns>
    private long BytesToMegabytes(long bytes)
    {
        return bytes / BytesInMegabyte;
    }


    /// <summary>
    /// Convierte un tamaño en bytes a gigabytes.
    /// </summary>
    /// <param name="bytes">El tamaño en bytes a convertir.</param>
    /// <returns>El tamaño en gigabytes.</returns>
    private long BytesToGigabytes(long bytes)
    {
        return bytes / BytesInGigabyte;
    }


    /// <summary>
    /// Convierte un tamaño en kilobytes a bytes.
    /// </summary>
    /// <param name="kilobytes">El tamaño en kilobytes a convertir.</param>
    /// <returns>El tamaño en bytes.</returns>
    private long KilobytesToBytes(long kilobytes)
    {
        return kilobytes * BytesInKilobyte;
    }

    /// <summary>
    /// Convierte un tamaño en megabytes a bytes.
    /// </summary>
    /// <param name="megabytes">El tamaño en megabytes a convertir.</param>
    /// <returns>El tamaño en bytes.</returns>
    private long MegabytesToBytes(long megabytes)
    {
        return megabytes * BytesInMegabyte;
    }

    /// <summary>
    /// Convierte un tamaño en gigabytes a bytes.
    /// </summary>
    /// <param name="gigabytes">El tamaño en gigabytes a convertir.</param>
    /// <returns>El tamaño en bytes.</returns>
    private long GigabytesToBytes(long gigabytes)
    {
        return gigabytes * BytesInGigabyte;
    }
}