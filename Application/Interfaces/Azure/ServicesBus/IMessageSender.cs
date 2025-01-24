using Shared.Configurations;

namespace Application.Interfaces.Azure.ServicesBus;

/// <summary>
/// Define la interfaz para enviar mensajes a colas o tópicos en un servicio de mensajería.
/// </summary>
public interface IMessageSender
{
    /// <summary>
    /// Envía un mensaje asincrónicamente a una cola o un tópico específico.
    /// </summary>
    /// <typeparam name="TRequest">El tipo del mensaje que se enviará.</typeparam>
    /// <param name="message">El mensaje que se enviará.</param>
    /// <param name="queue">El nombre de la cola o el tópico al que se enviará el mensaje.</param>
    /// <param name="cancellationToken">Un token para cancelar la operación de envío.</param>
    /// <param name="properties">
    /// Propiedades adicionales para el mensaje, como encabezados personalizados o metadatos específicos.
    /// Este parámetro es opcional y puede ser <c>null</c>.
    /// </param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica de envío del mensaje.
    /// </returns>
    Task SendMessageAsync<TRequest>(
        TRequest message, string queue,
        CancellationToken cancellationToken = default,
        AzureProperty? properties = null
    );
}