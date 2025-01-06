using Shared.Utils.Enums;

namespace Shared.Configurations;

public class AzureProperties
{
    /// <summary>
    /// El identificador único del mensaje.
    /// </summary>
    public string? MessageId { get; set; }

    /// <summary>
    /// El identificador de usuario asociado al mensaje.
    /// </summary>
    public byte[]? UserId { get; set; }

    /// <summary>
    /// La dirección de destino del mensaje.
    /// </summary>
    public string? To { get; set; }

    /// <summary>
    /// El asunto del mensaje.
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// La dirección para la respuesta del mensaje.
    /// </summary>
    public string? ReplyTo { get; set; }

    /// <summary>
    /// El identificador de correlación para vincular mensajes relacionados.
    /// </summary>
    public string? CorrelationId { get; set; }

    /// <summary>
    /// El tipo de contenido del mensaje (por ejemplo, application/json).
    /// </summary>
    public ContentType? ContentType { get; set; }

    /// <summary>
    /// La codificación del contenido del mensaje.
    /// </summary>
    public string? ContentEncoding { get; set; }

    /// <summary>
    /// La fecha y hora de expiración absoluta del mensaje.
    /// </summary>
    public DateTimeOffset? AbsoluteExpiryTime { get; set; }

    /// <summary>
    /// La fecha y hora de creación del mensaje.
    /// </summary>
    public DateTimeOffset? CreationTime { get; set; }

    /// <summary>
    /// El identificador de grupo del mensaje.
    /// </summary>
    public string? GroupId { get; set; }

    /// <summary>
    /// La secuencia de grupo del mensaje.
    /// </summary>
    public uint? GroupSequence { get; set; }

    /// <summary>
    /// El identificador del grupo para la respuesta.
    /// </summary>
    public string? ReplyToGroupId { get; set; }
    
    public string? PartitionKey { get; set; }
    public string? ReplyToSessionId { get; set; }
    public TimeSpan? TimeToLive { get; set; }
    public DateTimeOffset? ScheduledEnqueueTimeUtc { get; set; }
    public string? SessionId { get; set; }
    public BinaryData? Body { get; set; }
    public string? TransactionPartitionKey { get; set; }
    
}