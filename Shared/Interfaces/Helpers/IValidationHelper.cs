using System.Diagnostics.CodeAnalysis;

namespace Shared.Interfaces.Helpers;

/// <summary>
/// Proporciona métodos de validación para verificar la validez de los datos de entrada.
/// Esta clase encapsula la lógica de validación común utilizada en la aplicación.
/// </summary>
public interface IValidationHelper
{
    /// <summary>
    /// Verifica si un objeto de tipo referencia es nulo.
    /// </summary>
    /// <typeparam name="T">Tipo del objeto.</typeparam>
    /// <param name="value">El objeto a validar.</param>
    /// <returns>True si el objeto es nulo; de lo contrario, False.</returns>
    /// <remarks>Este método aplica únicamente a tipos de referencia.</remarks>
    bool IsNull<T>(T? value);
    
    /// <summary>
    /// Verifica si un valor de tipo valor nulo (nullable) es nulo.
    /// </summary>
    /// <typeparam name="T">Tipo del valor.</typeparam>
    /// <param name="value">El valor a validar.</param>
    /// <returns>True si el valor no tiene un valor asignado; de lo contrario, False.</returns>
    /// <remarks>Este método aplica únicamente a tipos de valor.</remarks>
    bool IsNull<T>(T? value) where T : struct;
    
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
    bool IsNotNull<T>([NotNullWhen(true)] T? value);
    
    /// <summary>
    /// Verifica si un valor de tipo valor nulo (nullable) tiene un valor asignado.
    /// </summary>
    /// <typeparam name="T">Tipo del valor.</typeparam>
    /// <param name="value">El valor a validar.</param>
    /// <returns>True si el valor tiene un valor asignado; de lo contrario, False.</returns>
    /// <remarks>El tipo debe ser nullable para usar este método (por ejemplo, int?, DateTime?).</remarks>
    bool IsNotNull<T>([NotNullWhen(true)] T? value) where T : struct;
    
    /// <summary>
    /// Verifica si un valor de tipo enumeración es válido.
    /// </summary>
    /// <typeparam name="T">Tipo de la enumeración.</typeparam>
    /// <param name="value">El valor de la enumeración a validar.</param>
    /// <returns>True si el valor es válido; de lo contrario, False.</returns>
    /// <remarks>Este método aplica únicamente a tipos de enumeración.</remarks>
    bool IsValidEnum<T>(T? value) where T : struct,Enum;

    /// <summary>
    /// Verifica si una colección contiene al menos un elemento.
    /// </summary>
    /// <typeparam name="T">El tipo de los elementos en la colección.</typeparam>
    /// <param name="collection">La colección a validar, que puede ser un array, una lista, o cualquier otra implementación de <c>IEnumerable&lt;T&gt;</c>.</param>
    /// <returns>True si la colección contiene al menos un elemento; de lo contrario, False. Si la colección es nula, también devuelve False.</returns>
    /// <remarks>Este método es genérico y puede ser utilizado con cualquier tipo que implemente <c>IEnumerable&lt;T&gt;</c>.</remarks>
    bool HasValues<T>(IEnumerable<T> collection);


}