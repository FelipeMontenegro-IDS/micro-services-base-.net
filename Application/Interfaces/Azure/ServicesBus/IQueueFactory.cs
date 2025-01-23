using Shared.Enums.Queue.Services;

namespace Application.Interfaces.Azure.ServicesBus;

/// <summary>
/// Define una interfaz base para crear instancias de objetos asociados con colas específicas de un micro-servicio, 
/// como objetos de solicitud (request) o respuesta (response), para gestionar las colas de manera más eficiente.
/// </summary>
public interface IQueueFactory
{
    /// <summary>
    /// Crea una instancia de un objeto que implementa <see cref="IBaseQueue{TQueue}"/> para representar una cola específica 
    /// de un micro-servicio dado.
    /// </summary>
    /// <typeparam name="TQueue">El tipo de enumerator que representa las colas asociadas al micro-servicio.</typeparam>
    /// <param name="microservice">El micro-servicio al que pertenece la cola.</param>
    /// <param name="queue">La cola específica, representada por el enumerator <typeparamref name="TQueue"/>.</param>
    /// <returns>
    /// Una instancia de <see cref="IBaseQueue{TQueue}"/> que representa la cola configurada para el micro-servicio y 
    /// el enumerator de cola proporcionados.
    /// </returns>
    public IBaseQueue<TQueue> Create<TQueue>(MicroService microservice, TQueue queue) where TQueue : Enum;

}