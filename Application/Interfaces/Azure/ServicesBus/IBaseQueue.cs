using Shared.Enums.Queue.Services;

namespace Application.Interfaces.Azure.ServicesBus;

/// <summary>
/// Define la interfaz base para gestionar las colas de un micro-servicio utilizando un enumerator para identificar las colas.
/// </summary>
/// <typeparam name="TQueue">El tipo de enumerator que representa las colas del micro-servicio.</typeparam>
public interface IBaseQueue<TQueue> where TQueue : Enum
{
    /// <summary>
    /// Obtiene o establece el microservicio asociado a la cola.
    /// </summary>
    public MicroService Microservice { get; set; }
    
    /// <summary>
    /// Obtiene o establece la cola específica del micro-servicio, representada por el enumerator <typeparamref name="TQueue"/>.
    /// </summary>
    public TQueue Queue { get; set; }
    
    /// <summary>
    /// Determina si una cola específica es válida para el micro-servicio proporcionado.
    /// </summary>
    /// <param name="microservice">El micro-servicio al que pertenece la cola.</param>
    /// <param name="queue">La cola a validar, representada por el enumerator <typeparamref name="TQueue"/>.</param>
    /// <returns><c>true</c> si la cola es válida para el micro-servicio; de lo contrario, <c>false</c>.</returns>
    bool IsValidQueue(MicroService microservice, TQueue queue);
    
    /// <summary>
    /// Obtiene el nombre de la cola como una cadena, en función del micro-servicio y el enumerator de cola configurados.
    /// </summary>
    /// <returns>El nombre de la cola como una cadena.</returns>
    string GetQueueName();
}