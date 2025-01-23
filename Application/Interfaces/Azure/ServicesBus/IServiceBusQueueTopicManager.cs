namespace Application.Interfaces.Azure.ServicesBus;

public interface IServiceBusQueueTopicManager
{
    

    /// <summary>
    /// Crea una cola en Azure Service Bus si no existe, con opciones personalizadas.
    /// </summary>
    /// <param name="queueName">El nombre de la cola a crear.</param>
    /// <param name="cancellationToken">cancelar</param>
    /// <returns>Una tarea vacía que representa la operación asincrónica.</returns>
    Task CreateQueueIfNotExists(string queueName, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Verifica si el nombre de la cola es válido.
    /// </summary>
    /// <param name="queueName">El nombre de la cola que se va a validar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el nombre de la cola es válido; de lo contrario, lanza una excepción <see cref="ArgumentException"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Se lanza cuando el nombre de la cola es nulo, vacío o solo contiene espacios en blanco.
    /// </exception>
    /// <remarks>
    /// Este método utiliza una expresión regular para validar el formato del nombre de la cola.
    /// Asegúrese de que el nombre de la cola cumpla con los requisitos establecidos por la expresión regular.
    /// </remarks>
    bool IsValidQueueName(string queueName);
    
    /// <summary>
    /// Crea un topic en Azure Service Bus si no existe, con opciones personalizadas.
    /// </summary>
    /// <param name="topicName">El nombre del topic a crear.</param>
    /// <param name="cancellationToken">token de cancelación.</param>
    /// <returns>Una tarea vacía que representa la operación asincrónica.</returns>
    Task CreateTopicIfNotExists(string topicName, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Verifica si el nombre del topic es válido.
    /// </summary>
    /// <param name="topicName">El nombre del topic que se va a validar.</param>
    /// <returns>
    /// Devuelve <c>true</c> si el nombre del topic es válido; de lo contrario, lanza una excepción <see cref="ArgumentException"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Se lanza cuando el nombre del topic es nulo, vacío o solo contiene espacios en blanco.
    /// </exception>
    /// <remarks>
    /// Este método utiliza una expresión regular para validar el formato del nombre del topic.
    /// Asegúrese de que el nombre del topic cumpla con los requisitos establecidos por la expresión regular.
    /// </remarks>
    bool IsValidTopicName(string topicName);

    /// <summary>
    /// Verifica si una cola con el nombre proporcionado existe.
    /// </summary>
    /// <param name="queueName">El nombre de la cola a verificar.</param>
    /// <param name="cancellationToken">El token de cancelación que puede ser utilizado para cancelar la operación (opcional).</param>
    /// <returns>Retorna <c>true</c> si la cola existe, de lo contrario <c>false</c>.</returns>
    Task<bool> QueueExists(string queueName,CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Verifica si un tópico con el nombre proporcionado existe.
    /// </summary>
    /// <param name="topicName">El nombre del tópico a verificar.</param>
    /// <param name="cancellationToken">El token de cancelación que puede ser utilizado para cancelar la operación (opcional).</param>
    /// <returns>Retorna <c>true</c> si el tópico existe, de lo contrario <c>false</c>.</returns>
    Task<bool> TopicExists(string topicName,CancellationToken cancellationToken = default); 
    
    /// <summary>
    /// Crea una suscripción para un tópico si no existe una suscripción con el nombre proporcionado.
    /// </summary>
    /// <param name="topicName">El nombre del tópico donde se creará la suscripción.</param>
    /// <param name="subscriptionName">El nombre de la suscripción a crear.</param>
    /// <param name="cancellationToken">El token de cancelación que puede ser utilizado para cancelar la operación (opcional).</param>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    Task CreateSubscriptionIfNotExists(string topicName, string subscriptionName, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Verifica si un nombre corresponde a un tópico. Si el nombre es válido, se verifica si existe un tópico con ese nombre.
    /// </summary>
    /// <param name="name">El nombre que se desea verificar como tópico.</param>
    /// <param name="cancellationToken">El token de cancelación que puede ser utilizado para cancelar la operación (opcional).</param>
    /// <returns>Retorna <c>true</c> si el nombre corresponde a un tópico existente, de lo contrario <c>false</c>.</returns>
    public Task<bool> IsTopic(string name,CancellationToken cancellationToken = default);

}