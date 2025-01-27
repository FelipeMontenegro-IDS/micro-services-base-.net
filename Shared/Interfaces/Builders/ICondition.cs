using System.Linq.Expressions;

namespace Shared.Interfaces.Builders;

/// <summary>
/// Define las operaciones para construir, agregar condiciones y evaluar expresiones lógicas para una entidad de tipo <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="TBuilder">Tipo del builder específico que implementa esta interfaz.</typeparam>
/// <typeparam name="T">Tipo de la entidad que se está evaluando.</typeparam>
public interface ICondition<out TBuilder, T>
{
    /// <summary>
    /// Construye una expresión booleana a partir de las condiciones añadidas.
    /// </summary>
    /// <returns>Una expresión booleana que representa la evaluación de las condiciones agregadas.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.Add(x => x.Age > 18).Add(x => x.Name == "John");
    /// var expression = builder.Build(); // La expresión resultante representará la condición (Age > 18) AND (Name == "John")
    /// </code>
    /// </example>
    Expression<Func<T, bool>> Build();

    /// <summary>
    /// Agrega una condición a la lista de condiciones, basándose en una expresión booleana.
    /// </summary>
    /// <param name="condition">La condición a agregar representada por una expresión booleana.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <exception cref="ArgumentNullException">Lanza una excepción si la condición es nula.</exception>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.Add(x => x.Age > 18); // Agrega una condición que verifica si la edad es mayor que 18
    /// </code>
    /// </example>
    TBuilder Add(Expression<Func<T, bool>> condition);

    /// <summary>
    /// Agrega una condición a la lista de condiciones basándose en una propiedad específica de la entidad de tipo <typeparamref name="T"/>
    /// y un predicado que evalúa un valor de tipo <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">El tipo del valor a evaluar.</typeparam>
    /// <param name="selector">Una expresión que selecciona la propiedad o valor de tipo <typeparamref name="TValue"/> de la entidad de tipo <typeparamref name="T"/>.</param>
    /// <param name="predicate">Una expresión booleana que evalúa el valor seleccionado por el <paramref name="selector"/>.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <exception cref="ArgumentNullException">Lanza una excepción si cualquiera de los parámetros es nulo.</exception>
    /// <example>
    /// <code>
    /// builder.Add(x => x.Age, age => age >= 18 );
    /// </code>
    /// </example>
    TBuilder Add<TValue>(Expression<Func<T, TValue>> selector, Expression<Func<TValue, bool>> predicate);

    /// <summary>
    /// Agrega un grupo de condiciones que se evaluarán como una sub-expresión. 
    /// Este grupo se puede agregar con otras condiciones lógicas, como AND u OR.
    /// </summary>
    /// <param name="groupBuilder">La acción que construye el grupo de condiciones.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.AddSubGroup(group => group.Add(x => x.Age > 18).Add(x => x.Name == "John"));
    /// // Este ejemplo agrega un subgrupo con las condiciones (Age > 18) AND (Name == "John")
    /// </code>
    /// </example>
    TBuilder AddSubGroup(Action<TBuilder> groupBuilder);

    /// <summary>
    /// Evalúa una condición específica sobre un objeto de tipo <typeparamref name="T"/>.
    /// </summary>
    /// <param name="obj">El objeto a evaluar.</param>
    /// <returns>El valor booleano resultante de la evaluación de las condiciones.</returns>
    /// <exception cref="InvalidOperationException">Lanza una excepción si hay un error al evaluar las condiciones.</exception>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.Add(x => x.Age > 18);
    /// bool result = builder.Evaluate(new Person { Age = 25 }); // Devuelve true
    /// </code>
    /// </example>
    public bool Evaluate(T obj);

    /// <summary>
    /// Evalúa una condición sobre cada elemento de una colección de objetos de tipo <typeparamref name="T"/>.
    /// </summary>
    /// <param name="collection">La colección de elementos a evaluar.</param>
    /// <returns>Una lista de elementos de la colección que cumplen con las condiciones especificadas.</returns>
    /// <exception cref="ArgumentException">Lanza una excepción si la colección es nula o vacía.</exception>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.Add(x => x.Age > 18);
    /// var result = builder.EvaluateAll(new List() { new Person { Age = 25 }, new Person { Age = 15 } });
    /// // Devuelve una lista que solo contiene la persona con edad 25
    /// </code>
    /// </example>
    public IEnumerable<T> EvaluateAll(IEnumerable<T> collection);

    /// <summary>
    /// Define una operación lógica "AND" entre las condiciones.
    /// </summary>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.Add(x => x.Age > 18).And().Add(x => x.Name == "John");
    /// // La expresión resultante será (Age > 18) AND (Name == "John")
    /// </code>
    /// </example>
    TBuilder And();

    /// <summary>
    /// Define una operación lógica "OR" entre las condiciones.
    /// </summary>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.Add(x => x.Age > 18).Or().Add(x => x.Name == "John");
    /// // La expresión resultante será (Age > 18) OR (Name == "John")
    /// </code>
    /// </example>
    TBuilder Or();

    /// <summary>
    /// Verifica que el valor no sea nulo.
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.NotNull(x => x.Name);
    /// // Verifica que la propiedad "Name" no sea nula
    /// </code>
    /// </example>
    TBuilder NotNull(Func<T, object?> selector);

    /// <summary>
    /// Verifica que el valor sea nulo.
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.Null(x => x.Name);
    /// // Verifica que la propiedad "Name" sea nula
    /// </code>
    /// </example>
    TBuilder Null(Func<T, object?> selector);

    /// <summary>
    /// Verifica que el valor esté dentro de un rango.
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <param name="min">El valor mínimo del rango.</param>
    /// <param name="max">El valor máximo del rango.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.InRange(x => x.Age, 18, 65);
    /// // Verifica que la propiedad "Age" esté entre 18 y 65
    /// </code>
    /// </example>
    TBuilder InRange(Func<T, int> selector, int min, int max);

    /// <summary>
    /// Verifica que la longitud de una cadena sea mayor o igual que el valor especificado.
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <param name="minLength">La longitud mínima que debe tener la cadena.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.MinLength(x => x.Name, 5);
    /// // Verifica que la propiedad "Name" tenga al menos 5 caracteres
    /// </code>
    /// </example>
    TBuilder MinLength(Func<T, string?> selector, int minLength);

    /// <summary>
    /// Verifica que la longitud de una cadena sea menor o igual que el valor especificado.
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <param name="maxLength">La longitud máxima que debe tener la cadena.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.MaxLength(x => x.Name, 10);
    /// // Verifica que la propiedad "Name" tenga como máximo 10 caracteres
    /// </code>
    /// </example>
    TBuilder MaxLength(Func<T, string?> selector, int maxLength);

    /// <summary>
    /// Verifica que el valor sea mayor que el valor especificado.
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <param name="value">El valor con el que comparar.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.GreaterThan(x => x.Age, 18);
    /// // Verifica que la propiedad "Age" sea mayor que 18
    /// </code>
    /// </example>
    TBuilder GreaterThan(Func<T, int> selector, int value);

    /// <summary>
    /// Verifica que el valor sea mayor o igual que el valor especificado.
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <param name="value">El valor con el que comparar.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.GreaterThanOrEqualTo(x => x.Age, 18);
    /// // Verifica que la propiedad "Age" sea mayor o igual a 18
    /// </code>
    /// </example>
    TBuilder GreaterThanOrEqualTo(Func<T, int> selector, int value);

    /// <summary>
    /// Verifica que el valor sea menor que el valor especificado.
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <param name="value">El valor con el que comparar.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.LessThan(x => x.Age, 18);
    /// // Verifica que la propiedad "Age" sea menor que 18
    /// </code>
    /// </example>
    TBuilder LessThan(Func<T, int> selector, int value);

    /// <summary>
    /// Verifica que el valor sea menor o igual que el valor especificado.
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <param name="value">El valor con el que comparar.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.LessThanOrEqualTo(x => x.Age, 18);
    /// // Verifica que la propiedad "Age" sea menor o igual a 18
    /// </code>
    /// </example>
    TBuilder LessThanOrEqualTo(Func<T, int> selector, int value);

    /// <summary>
    /// Verifica que el valor sea igual al valor especificado.
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <param name="value">El valor con el que comparar.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.EqualTo(x => x.Name, "John");
    /// // Verifica que la propiedad "Name" sea igual a "John"
    /// </code>
    /// </example>
    TBuilder EqualTo(Func<T, string?> selector, string value);

    /// <summary>
    /// Verifica que el valor coincida con una expresión regular.
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <param name="pattern">El patrón de la expresión regular.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.RegexMatch(x => x.Email, @"^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z]{2,}$");
    /// // Verifica que la propiedad "Email" coincida con el patrón de correo electrónico
    /// </code>
    /// </example>
    TBuilder RegexMatch(Func<T, string?> selector, string pattern);

    /// <summary>
    /// Verifica que la colección no esté vacía.
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.NotEmpty(x => x.Items);
    /// // Verifica que la propiedad "Items" no esté vacía
    /// </code>
    /// </example>
    TBuilder NotEmpty(Func<T, IEnumerable<object?>> selector);

    /// <summary>
    /// Verifica que el valor sea verdadero.
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.IsTrue(x => x.IsActive);
    /// // Verifica que la propiedad "IsActive" sea verdadera
    /// </code>
    /// </example>
    TBuilder IsTrue(Func<T, bool> selector);

    /// <summary>
    /// Verifica que el valor sea falso.
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.IsFalse(x => x.IsArchived);
    /// // Verifica que la propiedad "IsArchived" sea falsa
    /// </code>
    /// </example>
    TBuilder IsFalse(Func<T, bool> selector);

    /// <summary>
    /// Verifica que la propiedad esté vacía (cadena vacía o nula).
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.IsEmpty(x => x.Name);
    /// // Verifica que la propiedad "Name" esté vacía o nula
    /// </code>
    /// </example>
    TBuilder IsEmpty(Func<T, string?> selector);

    /// <summary>
    /// Verifica que la propiedad no esté vacía (no nula ni vacía).
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.IsNotEmpty(x => x.Name);
    /// // Verifica que la propiedad "Name" no esté vacía ni nula
    /// </code>
    /// </example>
    TBuilder IsNotEmpty(Func<T, string?> selector);

    /// <summary>
    /// Verifica que la fecha de la propiedad sea una fecha futura.
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.IsFutureDate(x => x.EventDate);
    /// // Verifica que la propiedad "EventDate" sea una fecha futura
    /// </code>
    /// </example>
    TBuilder IsFutureDate(Func<T, DateTime> selector);

    /// <summary>
    /// Verifica que la fecha de la propiedad sea una fecha pasada.
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.IsPastDate(x => x.EventDate);
    /// // Verifica que la propiedad "EventDate" sea una fecha pasada
    /// </code>
    /// </example>
    TBuilder IsPastDate(Func<T, DateTime> selector);

    /// <summary>
    /// Verifica que el valor de la propiedad sea positivo (mayor que cero).
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.IsPositive(x => x.Amount);
    /// // Verifica que la propiedad "Amount" sea positiva
    /// </code>
    /// </example>
    TBuilder IsPositive(Func<T, int> selector);

    /// <summary>
    /// Verifica que el valor de la propiedad sea negativo (menor que cero).
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.IsNegative(x => x.Amount);
    /// // Verifica que la propiedad "Amount" sea negativa
    /// </code>
    /// </example>
    TBuilder IsNegative(Func<T, int> selector);

    /// <summary>
    /// Verifica que la propiedad no sea igual a un valor específico.
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <param name="value">El valor con el que comparar.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.NotEqualTo(x => x.Age, 18);
    /// // Verifica que la propiedad "Age" no sea igual a 18
    /// </code>
    /// </example>
    TBuilder NotEqualTo(Func<T, int> selector, int value);


    /// <summary>
    /// Verifica que el valor de la propiedad sea mayor o igual que un valor mínimo.
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <param name="minValue">El valor mínimo permitido.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.MinValue(x => x.Amount, 100);
    /// // Verifica que la propiedad "Amount" sea mayor o igual a 100
    /// </code>
    /// </example>
    TBuilder MinValue(Func<T, int> selector, int minValue);

    /// <summary>
    /// Verifica que el valor de la propiedad sea menor o igual que un valor máximo.
    /// </summary>
    /// <param name="selector">El selector de la propiedad a verificar.</param>
    /// <param name="maxValue">El valor máximo permitido.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.MaxValue(x => x.Amount, 1000);
    /// // Verifica que la propiedad "Amount" sea menor o igual a 1000
    /// </code>
    /// </example> 
    TBuilder MaxValue(Func<T, int> selector, int maxValue);

    /// <summary>
    /// Agrega una condición que válida si el valor seleccionado corresponde a un formato de correo electrónico válido.
    /// </summary>
    /// <param name="selector">Una expresión que selecciona la propiedad o valor de tipo <see cref="string"/> de la entidad de tipo <typeparamref name="T"/> que se desea validar.</param>
    /// <returns>El builder actual para permitir el encadenamiento de métodos.</returns>
    /// <exception cref="ArgumentNullException">Lanza una excepción si el parámetro <paramref name="selector"/> es nulo.</exception>
    /// <example>
    /// <code>
    /// var builder = new ConditionBuilder();
    /// builder.Email(x => x.EmailAddress);
    /// var expression = builder.Build(); // La expresión resultante validará si el correo electrónico de la entidad es válido.
    /// </code>
    /// </example>
    TBuilder Email(Func<T, string?> selector);
}