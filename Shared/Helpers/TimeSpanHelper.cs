using System.Globalization;
using Shared.Enums;
using Shared.Providers;

namespace Shared.Helpers;

public static class TimeSpanHelper
{
    /// <summary>
    /// Crea un TimeSpan a partir de horas, minutos y segundos.
    /// </summary>
    public static TimeSpan CreateTimeSpan(int hours, int minutes, int seconds)
    {
        return new TimeSpan(hours, minutes, seconds);
    }
    
    /// <summary>
    /// Crea un TimeSpan a partir de días.
    /// </summary>
    public static TimeSpan CreateTimeSpanFromDays(int days)
    {
        return TimeSpan.FromDays(days);
    }

    /// <summary>
    /// Crea un TimeSpan a partir de horas.
    /// </summary>
    /// <param name="hours">Horas.</param>
    /// <returns>Un TimeSpan representando el tiempo especificado.</returns>
    public static TimeSpan CreateTimeSpanFromHours(double hours)
    {
        return TimeSpan.FromHours(hours);
    }

    /// <summary>
    /// Crea un TimeSpan a partir de minutos.
    /// </summary>
    /// <param name="minutes">Minutos.</param>
    /// <returns>Un TimeSpan representando el tiempo especificado.</returns>
    public static TimeSpan CreateTimeSpanFromMinutes(double minutes)
    {
        return TimeSpan.FromMinutes(minutes);
    }

    /// <summary>
    /// Crea un TimeSpan a partir de segundos.
    /// </summary>
    /// <param name="seconds">Segundos.</param>
    /// <returns>Un TimeSpan representando el tiempo especificado.</returns>
    public static TimeSpan CreateTimeSpanFromSeconds(double seconds)
    {
        return TimeSpan.FromSeconds(seconds);
    }

    
    /// <summary>
    /// Formatea un <see cref="TimeSpan"/> en una cadena personalizada según el formato especificado.
    /// </summary>
    /// <param name="timeSpan">El <see cref="TimeSpan"/> que se desea formatear.</param>
    /// <param name="format">El formato de <see cref="TimeSpanFormat"/> que se utilizará para la conversión.</param>
    /// <returns>Una cadena que representa el <see cref="TimeSpan"/> formateado según el formato especificado.</returns>
    /// <remarks>
    /// Este método utiliza la cultura "en-US" para el formateo de la cadena resultante.
    /// Asegúrese de que el formato proporcionado sea válido y esté definido en el proveedor de formatos de <see cref="TimeSpan"/>.
    /// </remarks>
    public static string FormatTimeSpan(TimeSpan timeSpan, TimeSpanFormat format)
    {
        return "";
        // return timeSpan.ToString(TimeSpanFormatProvider.GetTimeSpanFormat(format),new CultureInfo("en-US"));
    }
    
    /// <summary>
    /// Suma dos TimeSpans.
    /// </summary>
    /// <param name="first">El primer TimeSpan.</param>
    /// <param name="second">El segundo TimeSpan.</param>
    /// <returns>El TimeSpan resultante de la suma.</returns>
    public static TimeSpan AddTimeSpans(TimeSpan first, TimeSpan second)
    {
        return first.Add(second);
    }
    
    /// <summary>
    /// Resta un TimeSpan de otro.
    /// </summary>
    /// <param name="first">El TimeSpan del que se resta.</param>
    /// <param name="second">El TimeSpan que se resta.</param>
    /// <returns>El TimeSpan resultante de la resta.</returns>
    public static TimeSpan SubtractTimeSpans(TimeSpan first, TimeSpan second)
    {
        return first.Subtract(second);
    }
}