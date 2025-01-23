using System.Linq.Expressions;

namespace Application.Interfaces.Ardalis;

/// <summary>
/// Interfaz que define métodos para extender las funcionalidades de las especificaciones.
/// </summary>
/// <typeparam name="T">El tipo de entidad que la especificación manejará.</typeparam>
public interface ISpecificationMethods<T>
{
    /// <summary>
    /// Agrega una expresión de inclusión para cargar propiedades relacionadas.
    /// </summary>
    /// <param name="includeExpression">La expresión que define la propiedad relacionada a incluir.</param>
    void AddInclude(Expression<Func<T, object>> includeExpression);
    
    /// <summary>
    /// Agrega una expresión para ordenar los resultados de manera ascendente.
    /// </summary>
    /// <param name="orderByExpression">La expresión que define el campo por el cual ordenar.</param>
    void AddOrderBy(Expression<Func<T, object?>> orderByExpression);
    
    /// <summary>
    /// Agrega una expresión para ordenar los resultados de manera descendente.
    /// </summary>
    /// <param name="orderByDescendingExpression">La expresión que define el campo por el cual ordenar en orden descendente.</param>
    void AddOrderByDescending(Expression<Func<T, object?>> orderByDescendingExpression);
    
    /// <summary>
    /// Aplica la paginación a los resultados.
    /// </summary>
    /// <param name="skip">El número de elementos a omitir.</param>
    /// <param name="take">El número de elementos a tomar.</param>
    void ApplyPagination(int skip, int take);

    /// <summary>
    /// Agrega un criterio de búsqueda a la especificación.
    /// </summary>
    /// <param name="selector">La expresión que define el campo por el cual buscar.</param>
    /// <param name="searchTerm">El término de búsqueda.</param>
    /// <param name="searchGroup">Orden de prioridad de búsqueda.</param>
    public void AddSearch(Expression<Func<T, string>> selector, string searchTerm,int searchGroup);
}