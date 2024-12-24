using Shared.Utils.Enums;

namespace Shared.Utils.Generals;

public static class DateTimeHelper
{
    // Obtiene la fecha y hora actual en UTC
    public static DateTime GetCurrentUtcDateTime()
    {
        TimeZoneOption timeZoneOption = GetTimeZoneOption(); // Obtiene la zona horaria local
        TimeZoneInfo timeZoneInfo = GetTimeZoneInfo(timeZoneOption); // Convierte el enum a TimeZoneInfo

        DateTime utcNow = DateTime.UtcNow; // Obtiene la hora actual en UTC
        return TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZoneInfo); // Convierte a la zona horaria especificada
    }

    // Convierte una fecha y hora a UTC
    public static DateTime ConvertToUtc(DateTime localDateTime)
    {
        return TimeZoneInfo.ConvertTimeToUtc(localDateTime);
    }

    // Convierte una fecha y hora UTC a la hora local
    public static DateTime ConvertToLocal(DateTime utcDateTime)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, TimeZoneInfo.Local);
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
    public static DateTime ChangeTimeZone(DateTime dateTime, TimeZoneInfo newTimeZone)
    {
        DateTime utcDateTime = ConvertToUtc(dateTime);
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, newTimeZone);
    }

    // Obtiene la fecha y hora actual en una zona horaria específica
    public static DateTime GetCurrentDateTimeInTimeZone(TimeZoneInfo timeZone)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(GetCurrentUtcDateTime(), timeZone);
    }
    
    private static TimeZoneInfo GetTimeZoneInfo(TimeZoneOption timeZoneOption)
    {
        return timeZoneOption switch
        {
            TimeZoneOption.PacificStandardTime => TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"),
            TimeZoneOption.EasternStandardTime => TimeZoneInfo.FindSystemTimeZoneById("SA Eastern Standard Time"),
            TimeZoneOption.CentralStandardTime => TimeZoneInfo.FindSystemTimeZoneById("SA Central Standard Time"),
            TimeZoneOption.GreenwichMeanTime => TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"),
            TimeZoneOption.BritishSummerTime => TimeZoneInfo.FindSystemTimeZoneById("GMT Daylight Time"),
            TimeZoneOption.CentralEuropeanTime => TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"),
            TimeZoneOption.EasternEuropeanTime => TimeZoneInfo.FindSystemTimeZoneById("Eastern European Standard Time"),
            TimeZoneOption.IndiaStandardTime => TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"),
            TimeZoneOption.JapanStandardTime => TimeZoneInfo.FindSystemTimeZoneById("Japan Standard Time"),
            TimeZoneOption.AustralianEasternStandardTime => TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time"),
            TimeZoneOption.MountainStandardTime => TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"),
            TimeZoneOption.AtlanticStandardTime => TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time"),
            _ => throw new ArgumentOutOfRangeException(nameof(timeZoneOption), "Zona horaria no soportada.")
        };
    }
    private  static TimeZoneOption GetTimeZoneOption()
    {
        TimeZoneInfo localTimeZone = TimeZoneInfo.Local;

        // Mapeo de zonas horarias a tu enum
        return localTimeZone.Id switch
        {
            "SA Pacific Standard Time" => TimeZoneOption.PacificStandardTime,
            "SA Eastern Standard Time" => TimeZoneOption.EasternStandardTime,
            "SA Central Standard Time" => TimeZoneOption.CentralStandardTime,
            "GMT Standard Time" => TimeZoneOption.GreenwichMeanTime,
            "GMT Daylight Time" => TimeZoneOption.BritishSummerTime,
            "Central European Standard Time" => TimeZoneOption.CentralEuropeanTime,
            "Eastern European Standard Time" => TimeZoneOption.EasternEuropeanTime,
            "India Standard Time" => TimeZoneOption.IndiaStandardTime,
            "Japan Standard Time" => TimeZoneOption.JapanStandardTime,
            "AUS Eastern Standard Time" => TimeZoneOption.AustralianEasternStandardTime,
            "Mountain Standard Time" => TimeZoneOption.MountainStandardTime,
            "Atlantic Standard Time" => TimeZoneOption.AtlanticStandardTime,
            _ => throw new ArgumentOutOfRangeException("Zona horaria no soportada.")
        };
    }
}