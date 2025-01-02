using System.Diagnostics.CodeAnalysis;

namespace Shared.Utils.Helpers;

public class ValidationHelper
{
 /// <summary>
    /// Verifica si un objeto de tipo referencia es nulo.
    /// </summary>
    /// <typeparam name="T">Tipo del objeto.</typeparam>
    /// <param name="value">El objeto a validar.</param>
    /// <returns>True si el objeto es nulo; de lo contrario, False.</returns>
    /// <remarks>Este método aplica únicamente a tipos de referencia.</remarks>
    public static bool IsNull<T>(T? value) where T : class
    {
        return value == null;
    }

    /// <summary>
    /// Verifica si un objeto de tipo referencia no es nulo.
    /// </summary>
    /// <typeparam name="T">Tipo del objeto.</typeparam>
    /// <param name="value">El objeto a validar.</param>
    /// <returns>True si el objeto no es nulo; de lo contrario, False.</returns>
    /// <remarks>Este método aplica únicamente a tipos de referencia.</remarks>
    public static bool IsNotNull<T>([NotNullWhen(true)] T? value) where T : class
    {
        return value != null;
    }

    /// <summary>
    /// Verifica si un valor de tipo valor nulo (nullable) no tiene un valor asignado.
    /// </summary>
    /// <typeparam name="T">Tipo del valor.</typeparam>
    /// <param name="value">El valor a validar.</param>
    /// <returns>True si el valor no tiene un valor asignado; de lo contrario, False.</returns>
    /// <remarks>El tipo debe ser nullable para usar este método (por ejemplo, int?, DateTime?).</remarks>
    public static bool IsNull<T>(T? value) where T : struct
    {
        return !value.HasValue;
    }

    /// <summary>
    /// Verifica si un valor de tipo valor nulo (nullable) tiene un valor asignado.
    /// </summary>
    /// <typeparam name="T">Tipo del valor.</typeparam>
    /// <param name="value">El valor a validar.</param>
    /// <returns>True si el valor tiene un valor asignado; de lo contrario, False.</returns>
    /// <remarks>El tipo debe ser nullable para usar este método (por ejemplo, int?, DateTime?).</remarks>
    public static bool IsNotNull<T>([NotNullWhen(true)] T? value) where T : struct
    {
        return value.HasValue;
    }

}