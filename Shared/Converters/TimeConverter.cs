using Shared.Enums;

namespace Shared.Converters;

/// <summary>
/// Clase de ayuda para convertir unidades de tiempo entre segundos, minutos y horas.
/// </summary>
public class TimeConverter
{
    /// <summary>
    /// Representa el número de segundos en un minuto.
    /// </summary>
    private const int SecondsInMinute = 60;

    /// <summary>
    /// Representa el número de segundos en una hora.
    /// </summary>
    private const int SecondsInHour = 3600;
    
    
    /// <summary>
    /// Convierte un tiempo de una unidad a otra.
    /// </summary>
    /// <param name="time">El tiempo a convertir.</param>
    /// <param name="fromUnit">La unidad de origen del tiempo.</param>
    /// <param name="toUnit">La unidad de destino a la que se convertirá el tiempo.</param>
    /// <returns>El tiempo convertido en la unidad de destino.</returns>
    /// <exception cref="NotSupportedException">Se lanza si se proporciona una unidad no soportada.</exception>
    public decimal Convert(decimal time, TimeUnit fromUnit, TimeUnit toUnit)
    {
        decimal timeInSeconds = fromUnit switch
        {
            TimeUnit.Seconds => time,
            TimeUnit.Minutes => MinutesToSeconds(time),
            TimeUnit.Hours => HoursToSeconds(time),
            _ => throw new NotSupportedException("Unknown TimeUnit")
        };

        return toUnit switch
        {
            TimeUnit.Seconds => timeInSeconds,
            TimeUnit.Minutes => SecondsToMinutes(timeInSeconds),
            TimeUnit.Hours => SecondsToHours(timeInSeconds),
            _ => throw new NotSupportedException("Unknown TimeUnit")
        };
    }

    
    /// <summary>
    /// Convierte un tiempo en minutos a segundos.
    /// </summary>
    /// <param name="minutes">El tiempo en minutos a convertir.</param>
    /// <returns>El tiempo en segundos.</returns>
    private decimal MinutesToSeconds(decimal minutes)
    {
        return minutes * SecondsInMinute;
    }
    
    /// <summary>
    /// Convierte un tiempo en horas a segundos.
    /// </summary>
    /// <param name="hours">El tiempo en horas a convertir.</param>
    /// <returns>El tiempo en segundos.</returns>
    private decimal HoursToSeconds(decimal hours)
    {
        return hours * SecondsInHour;
    }

    /// <summary>
    /// Convierte un tiempo en segundos a minutos.
    /// </summary>
    /// <param name="seconds">El tiempo en segundos a convertir.</param>
    /// <returns>El tiempo en minutos.</returns>
    private decimal SecondsToMinutes(decimal seconds)
    {
        return seconds / SecondsInMinute;
    }
    
    
    /// <summary>
    /// Convierte un tiempo en segundos a horas.
    /// </summary>
    /// <param name="seconds">El tiempo en segundos a convertir.</param>
    /// <returns>El tiempo en horas.</returns>
    private decimal SecondsToHours(decimal seconds)
    {
        return seconds / SecondsInHour;
    }
}