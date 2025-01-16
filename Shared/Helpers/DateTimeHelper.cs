using System.Globalization;
using Shared.Constants;
using Shared.Enums;
using Shared.Interfaces.Helpers;
using Shared.Interfaces.Providers;

namespace Shared.Helpers;

/// <summary>
/// Proporciona métodos de ayuda para trabajar con fechas y horas.
/// Esta clase encapsula la lógica relacionada con la manipulación de fechas y horas, incluyendo la conversión de zonas horarias y el formato de fechas.
/// </summary>
public class DateTimeHelper : IDateTimeHelper
{
    private readonly ITimeZoneProvider _timeZoneProvider;
    private readonly IDateFormatProvider _dateFormatProvider;

    public DateTimeHelper(ITimeZoneProvider timeZoneProvider, IDateFormatProvider dateFormatProvider)
    {
        _timeZoneProvider = timeZoneProvider;
        _dateFormatProvider = dateFormatProvider;
    }

    /// <summary>
    /// Obtiene la fecha y hora actual en UTC.
    /// </summary>
    /// <returns>La fecha y hora actual en UTC.</returns>
    public DateTime GetCurrentUtcDateTime()
    {
        return DateTime.UtcNow;
    }
    /// <summary>
    /// Convierte una fecha y hora en formato UTC a la hora local según la opción de zona horaria especificada.
    /// </summary>
    /// <param name="utcDateTime">La fecha y hora en formato UTC que se desea convertir.</param>
    /// <param name="timeZoneOption">La opción de zona horaria que se utilizará para la conversión.</param>
    /// <returns>La fecha y hora convertida a la hora local del usuario.</returns>
    public DateTime ConvertTo(DateTime utcDateTime, TimeZoneOption timeZoneOption)
    {
        TimeZoneInfo timeZoneInfo = _timeZoneProvider.GetTimeZoneInfo(timeZoneOption);
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZoneInfo);
    }

    public DateTimeOffset ConvertTo(DateTimeOffset utcDateTime, TimeZoneOption timeZoneOption)
        {
        DateTime utcTime = utcDateTime.UtcDateTime;
        
        TimeZoneInfo timeZoneInfo = _timeZoneProvider.GetTimeZoneInfo(timeZoneOption);

        DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZoneInfo);
        
        return new DateTimeOffset(localTime, timeZoneInfo.GetUtcOffset(localTime));

    }

    /// <summary>
    /// Obtiene la fecha y hora actual en una zona horaria específica.
    /// </summary>
    /// <param name="timeZoneOption">La opción de zona horaria que se utilizará para obtener la fecha y hora actual.</param>
    /// <returns>La fecha y hora actual en la zona horaria especificada.</returns>
    public DateTime GetCurrentDateTimeInTimeZone(TimeZoneOption timeZoneOption)
    {
        TimeZoneInfo timeZoneInfo = _timeZoneProvider.GetTimeZoneInfo(timeZoneOption);
        return TimeZoneInfo.ConvertTimeFromUtc(GetCurrentUtcDateTime(), timeZoneInfo);
    }

    /// <summary>
    /// Formatea una fecha en un formato específico utilizando el proveedor de formatos de fecha.
    /// </summary>
    /// <param name="dateTime">La fecha y hora que se desea formatear.</param>
    /// <param name="format">El formato específico que se utilizará para la fecha.</param>
    /// <returns>Una cadena que representa la fecha formateada según el formato especificado.</returns>
    public string FormatDate(DateTime dateTime, DateFormat format)
    {
        return dateTime.ToString(_dateFormatProvider.GetValue(format, DateFormatConstant.IsoDate),
            new CultureInfo("en-US"));
    }

    /// <summary>
    /// Calcula la diferencia en días entre dos fechas.
    /// </summary>
    /// <param name="startDate">La fecha de inicio para el cálculo.</param>
    /// <param name="endDate">La fecha de fin para el cálculo.</param>
    /// <returns>El número de días entre las dos fechas.</returns>
    public int DaysBetween(DateTime startDate, DateTime endDate)
    {
        return (endDate - startDate).Days;
    }

    /// <summary>
    /// Verifica si una fecha es un fin de semana (sábado o domingo).
    /// </summary>
    /// <param name="dateTime">La fecha que se desea verificar.</param>
    /// <returns>Devuelve <c>true</c> si la fecha es un fin de semana; de lo contrario, devuelve <c>false</c>.</returns>
    public bool IsWeekend(DateTime dateTime)
    {
        return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
    }

    /// <summary>
    /// Agrega un número específico de días a una fecha dada.
    /// </summary>
    /// <param name="dateTime">La fecha a la que se le agregarán los días.</param>
    /// <param name="days">El número de días que se desea agregar a la fecha.</param>
    /// <returns>Una nueva fecha que resulta de agregar el número especificado de días a la fecha original.</returns>
    public DateTime AddDays(DateTime dateTime, int days)
    {
        return dateTime.AddDays(days);
    }

    /// <summary>
    /// Resta un número específico de días de una fecha dada.
    /// </summary>
    /// <param name="dateTime">La fecha de la que se desea restar los días.</param>
    /// <param name="days">El número de días que se desea restar de la fecha.</param>
    /// <returns>Una nueva fecha que resulta de restar el número especificado de días a la fecha original.</returns>
    public DateTime SubtractDays(DateTime dateTime, int days)
    {
        return dateTime.AddDays(-days);
    }

    /// <summary>
    /// Obtiene el primer día del mes de una fecha dada.
    /// </summary>
    /// <param name="dateTime">La fecha de la que se desea obtener el primer día del mes.</param>
    /// <returns>Una nueva fecha que representa el primer día del mes de la fecha original.</returns>
    public DateTime GetFirstDayOfMonth(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, 1);
    }

    /// <summary>
    /// Obtiene el último día del mes de una fecha dada.
    /// </summary>
    /// <param name="dateTime">La fecha de la que se desea obtener el último día del mes.</param>
    /// <returns>Una nueva fecha que representa el último día del mes de la fecha original.</returns>
    public DateTime GetLastDayOfMonth(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
    }

    /// <summary>
    /// Verifica si una fecha es un día laborable.
    /// </summary>
    /// <param name="dateTime">La fecha que se desea verificar.</param>
    /// <returns>Devuelve true si la fecha es un día laborable; de lo contrario, devuelve false.</returns>
    public bool IsWeekday(DateTime dateTime)
    {
        return !IsWeekend(dateTime);
    }

    /// <summary>
    /// Obtiene la cantidad de semanas entre dos fechas.
    /// </summary>
    /// <param name="startDate">La fecha de inicio del intervalo.</param>
    /// <param name="endDate">La fecha de fin del intervalo.</param>
    /// <returns>El número de semanas completas entre las dos fechas.</returns>
    public int WeeksBetween(DateTime startDate, DateTime endDate)
    {
        return (int)((endDate - startDate).TotalDays / 7);
    }

    /// <summary>
    /// Obtiene la fecha del inicio de la semana para una fecha dada.
    /// </summary>
    /// <param name="dateTime">La fecha de la que se desea obtener el inicio de la semana.</param>
    /// <returns>Una nueva fecha que representa el primer día de la semana (lunes) de la fecha original.</returns>
    public DateTime GetStartOfWeek(DateTime dateTime)
    {
        int diff = dateTime.DayOfWeek - DayOfWeek.Monday;
        if (diff < 0) diff += 7;
        return dateTime.AddDays(-diff).Date;
    }

    /// <summary>
    /// Obtiene la fecha de fin de la semana (domingo) para una fecha dada.
    /// </summary>
    /// <param name="dateTime">La fecha de la que se desea obtener el fin de la semana.</param>
    /// <returns>Una nueva fecha que representa el último día de la semana (domingo) de la fecha original.</returns>
    public DateTime GetEndOfWeek(DateTime dateTime)
    {
        int diff = DayOfWeek.Sunday - dateTime.DayOfWeek;
        if (diff < 0) diff += 7;
        return dateTime.AddDays(diff).Date;
    }

    /// <summary>
    /// Verifica si una fecha dada se encuentra dentro de un rango de fechas especificado.
    /// </summary>
    /// <param name="dateTime">La fecha que se va a verificar.</param>
    /// <param name="startDate">La fecha de inicio del rango.</param>
    /// <param name="endDate">La fecha de finalización del rango.</param>
    /// <returns>Devuelve true si la fecha está dentro del rango, de lo contrario, false.</returns>
    /// <remarks>
    /// Esta función considera que las fechas de inicio y final son inclusivas.
    /// </remarks>
    public bool IsDateInRange(DateTime dateTime, DateTime startDate, DateTime endDate)
    {
        return dateTime >= startDate && dateTime <= endDate;
    }

    /// <summary>
    /// Obtiene la cantidad de días en un mes específico de un año dado.
    /// </summary>
    /// <param name="year">El año del mes que se desea verificar.</param>
    /// <param name="month">El mes del que se desea obtener la cantidad de días (1-12).</param>
    /// <returns>El número de días en el mes especificado del año dado.</returns>
    public int GetDaysInMonth(int year, int month)
    {
        return DateTime.DaysInMonth(year, month);
    }


    /// <summary>
    /// Clona un objeto DateTime con una nueva zona horaria.
    /// </summary>
    /// <param name="dateTime">La fecha y hora que se desea clonar.</param>
    /// <param name="timeZoneOption">La opción de zona horaria actual del objeto DateTime.</param>
    /// <param name="newTimeZone">La nueva zona horaria a la que se desea convertir.</param>
    /// <returns>Un nuevo objeto DateTime que representa la misma fecha y hora en la nueva zona horaria.</returns>
    public DateTime ChangeTimeZone(DateTime dateTime, TimeZoneOption timeZoneOption,
        TimeZoneInfo newTimeZone)
    {
        DateTime utcDateTime = ConvertTo(dateTime, timeZoneOption);
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, newTimeZone);
    }
}