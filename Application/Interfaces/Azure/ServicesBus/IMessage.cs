using Azure.Messaging.ServiceBus;
using Shared.Enums.Data;

namespace Application.Interfaces.Azure.ServicesBus;

public interface IMessage
{ 
    /// <summary>
    /// Envía una solicitud a una cola de solicitudes y espera una respuesta de una cola de respuestas.
    /// Permite configurar la política de reintentos y procesar la respuesta una vez recibida.
    /// </summary>
    /// <typeparam name="TRequest">Tipo de mensaje de solicitud</typeparam>
    /// <typeparam name="TResponse">Tipo de mensaje de respuesta</typeparam>
    /// <param name="request">El mensaje de solicitud a enviar</param>
    /// <param name="queueRequest">El nombre de la cola de solicitudes</param>
    /// <param name="queueResponse">El nombre de la cola de respuestas</param>
    /// <param name="options">Opciones para el procesador de mensajes</param>
    /// <param name="cancellationToken">Token de cancelación para la operación</param>
    /// <param name="headers">Encabezados adicionales para el mensaje (opcional)</param>
    /// <param name="retryPolicyDefault">Política de reintentos a utilizar (opcional)</param>
    /// <returns>Un Task que se resuelve con la respuesta procesada</returns>
    Task<TResponse> ProcessRequestAsync<TRequest, TResponse>(
        TRequest request,
        string queueRequest,
        string queueResponse,
        ServiceBusProcessorOptions options,
        CancellationToken cancellationToken = default,
        IDictionary<string, object>? headers = null,
        RetryPolicyDefault retryPolicyDefault = RetryPolicyDefault.NoRetries) where TRequest : class where TResponse : class;

    
    
    /// <summary>
    /// Recibe un mensaje de la cola de solicitudes, lo procesa y luego envía una respuesta a la cola de respuestas.
    /// Se utiliza para manejar solicitudes y generar respuestas dinámicamente.
    /// </summary>
    /// <typeparam name="TRequest">Tipo de mensaje de solicitud</typeparam>
    /// <typeparam name="TResponse">Tipo de mensaje de respuesta</typeparam>
    /// <param name="queueRequest">El nombre de la cola de solicitudes</param>
    /// <param name="queueResponse">El nombre de la cola de respuestas</param>
    /// <param name="options">Opciones para el procesador de mensajes</param>
    /// <param name="processResponseToSend">Función que procesa la solicitud y genera una respuesta</param>
    /// <param name="cancellationToken">Token de cancelación para la operación</param>
    /// <param name="headers">Encabezados adicionales para el mensaje (opcional)</param>
    /// <param name="retryPolicyDefault">Política de reintentos a utilizar (opcional)</param>
    /// <returns>Un Task que se resuelve cuando el proceso de recepción y respuesta se completa</returns>
    Task ReceiveAndResponseAsync<TRequest, TResponse>(
        string queueRequest,
        string queueResponse,
        ServiceBusProcessorOptions options,
        Func<TRequest, CancellationToken, Task<TResponse>> processResponseToSend,
        CancellationToken cancellationToken = default,
        IDictionary<string, object>? headers = null,
        RetryPolicyDefault retryPolicyDefault = RetryPolicyDefault.NoRetries) where TRequest : class where TResponse : class;
    
    
    /// <summary>
    /// Envía un mensaje a una cola específica.
    /// Permite configurar la política de reintentos y agregar encabezados adicionales al mensaje.
    /// </summary>
    /// <typeparam name="T">Tipo de mensaje a enviar</typeparam>
    /// <param name="message">El mensaje a enviar</param>
    /// <param name="queue">El nombre de la cola a la que se enviará el mensaje</param>
    /// <param name="headers">Encabezados adicionales para el mensaje (opcional)</param>
    /// <param name="cancellationToken">Token de cancelación para la operación</param>
    /// <param name="retryPolicyDefault">Política de reintentos a utilizar (opcional)</param>
    /// <returns>Un Task que se resuelve cuando el mensaje ha sido enviado</returns>
    Task SendAsync<T>(
        T message,
        string queue,
        IDictionary<string, object>? headers = null,
        CancellationToken cancellationToken = default,
        RetryPolicyDefault retryPolicyDefault = RetryPolicyDefault.NoRetries) where T : class;

    
    /// <summary>
    /// Recibe un mensaje de una cola o tema, lo procesa con una función personalizada y devuelve un mensaje procesado.
    /// Este método permite trabajar con mensajes de una manera más flexible y personalizada.
    /// </summary>
    /// <typeparam name="T">Tipo de mensaje a recibir y procesar</typeparam>
    /// <param name="queueOrTopicName">El nombre de la cola o tema desde el cual se recibirá el mensaje</param>
    /// <param name="subscriptionName">El nombre de la suscripción (si es un tema)</param>
    /// <param name="processMessageAsync">Función para procesar el mensaje recibido</param>
    /// <param name="options">Opciones para el procesador de mensajes</param>
    /// <param name="cancellationToken">Token de cancelación para la operación</param>
    /// <returns>Un Task que se resuelve con el mensaje procesado</returns>
    public Task<T> ProcessAndReturnMessageAsync<T>(
        string queueOrTopicName,
        string? subscriptionName,
        Func<T, CancellationToken, Task<T>> processMessageAsync,
        ServiceBusProcessorOptions options,
        CancellationToken cancellationToken = default)
        where T : class;
}