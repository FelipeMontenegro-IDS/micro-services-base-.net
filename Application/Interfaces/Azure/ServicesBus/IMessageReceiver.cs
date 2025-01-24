using Azure.Messaging.ServiceBus;

namespace Application.Interfaces.Azure.ServicesBus;

/// <summary>
/// Define la interfaz para la recepción y manejo de mensajes desde colas o tópicos en un servicio de mensajería.
/// </summary>
public interface IMessageReceiver
{
    /// <summary>
    /// Registra un controlador para procesar mensajes de una cola o un tópico específico, con opciones personalizadas 
    /// para el procesamiento de mensajes y manejo de errores.
    /// </summary>
    /// <typeparam name="T">El tipo de mensaje que se espera procesar.</typeparam>
    /// <param name="queueOrTopicName">El nombre de la cola o el tópico desde el cual se recibirán los mensajes.</param>
    /// <param name="subscriptionName">
    /// El nombre de la suscripción, en caso de que el <paramref name="queueOrTopicName"/> sea un tópico. 
    /// Puede ser <c>null</c> si se trata de una cola.
    /// </param>
    /// <param name="processMessageAsync">
    /// Una función asincrónica que procesa el mensaje recibido. 
    /// Recibe el mensaje de tipo <typeparamref name="T"/> y un <see cref="CancellationToken"/>.
    /// </param>
    /// <param name="options">
    /// Opciones de configuración para el procesador de mensajes, como número de reintentos y concurrencia.
    /// </param>
    /// <param name="cancellationToken">Un token para cancelar el registro del controlador de mensajes.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica de registro del controlador de mensajes.
    /// </returns>
    Task RegisterMessageHandler<T>(
        string queueOrTopicName,
        string? subscriptionName,
        Func<T, CancellationToken, Task> processMessageAsync,
        ServiceBusProcessorOptions options,
        CancellationToken cancellationToken = default
    ) where T : class;
}