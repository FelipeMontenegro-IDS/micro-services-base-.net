using System.Diagnostics.CodeAnalysis;
using Shared.Interfaces.Helpers;

namespace Shared.Helpers;

/// <summary>
/// Proporciona métodos de validación para verificar la validez de los datos de entrada.
/// Esta clase encapsula la lógica de validación común utilizada en la aplicación.
/// </summary>
public class ValidationHelper : IValidationHelper
{
    /// <summary>
    /// Verifica si un objeto de tipo referencia es nulo.
    /// </summary>
    /// <typeparam name="T">Tipo del objeto.</typeparam>
    /// <param name="value">El objeto a validar.</param>
    /// <returns>True si el objeto es nulo; de lo contrario, False.</returns>
    /// <remarks>Este método aplica únicamente a tipos de referencia.</remarks>
    public bool IsNull<T>(T? value)
    {
        return value == null;
    }
    
    /// <summary>
    /// Verifica si un valor de tipo valor nulo (nullable) es nulo.
    /// </summary>
    /// <typeparam name="T">Tipo del valor.</typeparam>
    /// <param name="value">El valor a validar.</param>
    /// <returns>True si el valor no tiene un valor asignado; de lo contrario, False.</returns>
    /// <remarks>Este método aplica únicamente a tipos de valor.</remarks>
    public bool IsNull<T>(T? value) where T : struct
    {
        return !value.HasValue;
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
    /// <remarks>
    /// Esta función es compatible con tipos de valor nullables y tipos de referencia.
    /// </remarks>
    public bool IsNotNull<T>([NotNullWhen(true)] T? value)
    {
        return value != null; 
    }

    /// <summary>
    /// Verifica si un valor de tipo valor nulo (nullable) tiene un valor asignado.
    /// </summary>
    /// <typeparam name="T">Tipo del valor.</typeparam>
    /// <param name="value">El valor a validar.</param>
    /// <returns>True si el valor tiene un valor asignado; de lo contrario, False.</returns>
    /// <remarks>El tipo debe ser nullable para usar este método (por ejemplo, int?, DateTime?).</remarks>
    public bool IsNotNull<T>([NotNullWhen(true)] T? value) where T : struct
    {
        return value.HasValue;
    }
    
    /// <summary>
    /// Verifica si un valor de tipo enumeración es válido.
    /// </summary>
    /// <typeparam name="T">Tipo de la enumeración.</typeparam>
    /// <param name="value">El valor de la enumeración a validar.</param>
    /// <returns>True si el valor es válido; de lo contrario, False.</returns>
    /// <remarks>Este método aplica únicamente a tipos de enumeración.</remarks>
    public bool IsValidEnum<T>(T? value) where T : struct,Enum
    {
        return IsNotNull(value) && Enum.IsDefined(typeof(T), value);
    }
}