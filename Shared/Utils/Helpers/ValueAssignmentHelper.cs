namespace Shared.Utils.Helpers;

public static class ValueAssignmentHelper
{
    /// <summary>
    /// Asigna un valor a una acción si no es null ni una cadena vacía.
    /// </summary>
    /// <param name="assignAction">Acción para asignar el valor.</param>
    /// <param name="value">Valor a validar y asignar.</param>
    /// <example>
    /// Ejemplo de uso:
    /// <code>
    /// ValueAssignmentHelper.SetIfNotNullOrEmpty(value => someObject.Property = value, "Hola mundo");
    /// </code>
    /// </example>
    public static void SetIfNotNullOrEmpty(Action<string> assignAction, string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            assignAction(value);
        }
    }

    /// <summary>
    /// Asigna un valor a una acción si no es null.
    /// </summary>
    /// <typeparam name="T">Tipo del valor.</typeparam>
    /// <param name="assignAction">Acción para asignar el valor.</param>
    /// <param name="value">Valor a validar y asignar.</param>
    /// <example>
    /// Ejemplo de uso:
    /// <code>
    /// ValueAssignmentHelper.SetIfNotNull(obj => obj.DoSomething(), myObject);
    /// </code>
    /// </example>
    public static void SetIfNotNull<T>(Action<T> assignAction, T? value) where T : class
    {
        if (ValidationHelper.IsNotNull(value))
        {
            assignAction(value);
        }
    }


    public static void SetIfNotNull<T>(Action<T?> assignAction, T? value) where T : struct
    {
        if (ValidationHelper.IsNotNull(value)) // Para tipo valor
        {
            assignAction(value);
        }
    }

    /// <summary>
    /// Asigna un valor a una acción si no es null y cumple con una condición.
    /// </summary>
    /// <typeparam name="T">Tipo del valor (Referencia).</typeparam>
    /// <param name="assignAction">Acción para asignar el valor.</param>
    /// <param name="value">Valor a validar y asignar.</param>
    /// <param name="predicate">Condición que debe cumplir el valor.</param>
    /// <example>
    /// Ejemplo de uso:
    /// <code>
    /// ValueAssignmentHelper.SetIf(value => someObject.Value = value, someValue, v => v > 10);
    /// </code>
    /// </example>
    public static void SetIf<T>(Action<T> assignAction, T? value, Func<T, bool> predicate) where T : class
    {
        if (ValidationHelper.IsNotNull(value) && predicate(value))
        {
            assignAction(value);
        }
    }

    /// <summary>
    /// Asigna un valor a una acción si no es null y cumple con una condición.
    /// </summary>
    /// <typeparam name="T">Tipo del valor.</typeparam>
    /// <param name="assignAction">Acción para asignar el valor.</param>
    /// <param name="value">Valor a validar y asignar.</param>
    /// <param name="predicate">Condición que debe cumplir el valor.</param>
    /// <example>
    /// Ejemplo de uso:
    /// <code>
    /// ValueAssignmentHelper.SetIf(value => someObject.Value = value, someValue, v => v > 10);
    /// </code>
    /// </example>
    public static void SetIf<T>(Action<T> assignAction, T? value, Func<T, bool> predicate)
        where T : struct // Esto permite tipos de valor
    {
        if (value.HasValue && predicate(value.Value)) // Para tipos de valor, verificamos si tiene valor
        {
            assignAction(value.Value); 
        }
    }

    /// <summary>
    /// Convierte un valor a su representación de cadena y lo asigna si no es null ni vacío.
    /// </summary>
    /// <typeparam name="T">Tipo del valor.</typeparam>
    /// <param name="assignAction">Acción para asignar el valor.</param>
    /// <param name="value">Valor a convertir y asignar.</param>
    /// <example>
    /// Ejemplo de uso:
    /// <code>
    /// ValueAssignmentHelper.SetToStringIfNotNull(value => someObject.StringValue = value, someIntValue);
    /// </code>
    /// </example>  
    public static void SetToStringIfNotNull<T>(Action<string> assignAction, T? value) where T : class
    {
        if (ValidationHelper.IsNotNull(value))
        {
            var stringValue = value.ToString();
            if (!string.IsNullOrEmpty(stringValue))
            {
                assignAction(stringValue);
            }
        }
    }

    /// <summary>
    /// Asigna un valor por defecto si el valor original es null.
    /// </summary>
    /// <typeparam name="T">Tipo del valor.</typeparam>
    /// <param name="assignAction">Acción para asignar el valor.</param>
    /// <param name="value">Valor original.</param>
    /// <param name="defaultValue">Valor por defecto.</param>
    /// <example>
    /// Ejemplo de uso:
    /// <code>
    /// ValueAssignmentHelper.SetDefaultIfNull(value => someObject.Property = value, null, "Valor por defecto");
    /// </code>
    /// </example>
    public static void SetDefaultIfNull<T>(Action<T> assignAction, T? value, T defaultValue)
    {
        assignAction(value ?? defaultValue);
    }
}