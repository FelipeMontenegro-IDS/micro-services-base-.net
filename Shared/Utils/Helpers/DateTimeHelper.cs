using Shared.Utils.Collections;
using Shared.Utils.Enums;

namespace Shared.Utils.Helpers;

public static class DateTimeHelper
{
    // Obtiene la fecha y hora actual en UTC
    public static DateTime GetCurrentUtcDateTime()
    {
        return DateTime.UtcNow;
    }

    // Convierte una fecha y hora a UTC desde la zona horaria local del usuario
    public static DateTime ConvertToUtc(DateTime localDateTime,
        string timeZoneOption)
    {
        TimeZoneOption timeZone = CollectionTimeZone.GetTimeZoneOption(timeZoneOption);
        TimeZoneInfo timeZoneInfo = CollectionTimeZone.GetTimeZoneInfo(timeZone);
        return TimeZoneInfo.ConvertTimeToUtc(localDateTime, timeZoneInfo);
    }

    // Convierte una fecha y hora UTC a la hora local de un usuario
    public static DateTime ConvertToLocal(DateTime utcDateTime,
        string timeZoneOption)
    {
        TimeZoneOption timeZone = CollectionTimeZone.GetTimeZoneOption(timeZoneOption);
        TimeZoneInfo timeZoneInfo = CollectionTimeZone.GetTimeZoneInfo(timeZone);
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZoneInfo);
    }

    // Obtiene la fecha y hora actual en una zona horaria específica
    public static DateTime GetCurrentDateTimeInTimeZone(string timeZoneOption)
    {
        TimeZoneOption timeZone = CollectionTimeZone.GetTimeZoneOption(timeZoneOption);
        TimeZoneInfo timeZoneInfo = CollectionTimeZone.GetTimeZoneInfo(timeZone);
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);
    }

    // Formatea una fecha en un formato específico
    public static string FormatDate(DateTime dateTime, string format)
    {
        return dateTime.ToString(format);
    }

    // Calcula la diferencia en días entre dos fechas
    public static int DaysBetween(DateTime startDate, DateTime endDate)
    {
        return (endDate - startDate).Days;
    }

    // Verifica si una fecha es un fin de semana
    public static bool IsWeekend(DateTime dateTime)
    {
        return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
    }

    // Agrega días a una fecha
    public static DateTime AddDays(DateTime dateTime, int days)
    {
        return dateTime.AddDays(days);
    }

    // Resta días de una fecha
    public static DateTime SubtractDays(DateTime dateTime, int days)
    {
        return dateTime.AddDays(-days);
    }

    // Obtiene el primer día del mes de una fecha
    public static DateTime GetFirstDayOfMonth(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, 1);
    }

    // Obtiene el último día del mes de una fecha
    public static DateTime GetLastDayOfMonth(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
    }

    // Verifica si una fecha es un día laborable
    public static bool IsWeekday(DateTime dateTime)
    {
        return !IsWeekend(dateTime);
    }

    // Obtiener la cantidad de semanas entre dos fechas
    public static int WeeksBetween(DateTime startDate, DateTime endDate)
    {
        return (int)((endDate - startDate).TotalDays / 7);
    }

    // Obtiene la fecha de inicio de la semana (lunes) para una fecha dada
    public static DateTime GetStartOfWeek(DateTime dateTime)
    {
        int diff = dateTime.DayOfWeek - DayOfWeek.Monday;
        if (diff < 0) diff += 7;
        return dateTime.AddDays(-diff).Date;
    }

    // Obtiene la fecha de fin de la semana (domingo) para una fecha dada
    public static DateTime GetEndOfWeek(DateTime dateTime)
    {
        int diff = DayOfWeek.Sunday - dateTime.DayOfWeek;
        if (diff < 0) diff += 7;
        return dateTime.AddDays(diff).Date;
    }

    // Verifica si una fecha está dentro de un rango
    public static bool IsDateInRange(DateTime dateTime, DateTime startDate, DateTime endDate)
    {
        return dateTime >= startDate && dateTime <= endDate;
    }

    // Obtiene la cantidad de días en un mes específico
    public static int GetDaysInMonth(int year, int month)
    {
        return DateTime.DaysInMonth(year, month);
    }


    // Clona un objeto DateTime con una nueva zona horaria
    public static DateTime ChangeTimeZone(DateTime dateTime, string timeZoneOption,
        TimeZoneInfo newTimeZone)
    {
        // Asegúrate de pasar la zona horaria para la conversión
        DateTime utcDateTime = ConvertToUtc(dateTime, timeZoneOption);
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, newTimeZone);
    }

    // Obtiene la fecha y hora actual en una zona horaria específica
    public static DateTime GetCurrentDateTimeInTimeZone(TimeZoneInfo timeZone)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(GetCurrentUtcDateTime(), timeZone);
    }
}