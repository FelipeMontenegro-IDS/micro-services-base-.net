using System.Diagnostics.CodeAnalysis;

namespace Shared.Helpers;

public static class ValidationHelper
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
    /// Verifica si un valor es nulo o no.
    /// </summary>
    /// <typeparam name="T">Tipo del valor a verificar.</typeparam>
    /// <param name="value">El valor a evaluar.</param>
    /// <returns>
    ///   <c>true</c> si el valor es nulo (null) o no tiene un valor asignado; 
    ///   <c>false</c> en caso contrario.
    /// </returns>
    /// <remarks T=".Default.Equals` para una comparación precisa.">
    /// Esta función es compatible con tipos de valor nullables y tipos de referencia.
    /// Utiliza `EqualityComparer
    /// </remarks>
    public static bool IsNotNull<T>([NotNullWhen(true)] T? value)
    {
        return !EqualityComparer<T>.Default.Equals(value, default(T));
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