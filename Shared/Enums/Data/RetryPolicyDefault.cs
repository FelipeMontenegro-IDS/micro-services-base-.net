namespace Shared.Enums.Data;

/// <summary>
/// Representa las políticas de reintentos predeterminadas para operaciones que pueden fallar.
/// </summary>
/// <remarks>
/// Este enum define una lista de configuraciones de reintentos que pueden ser utilizadas
/// para manejar fallos en operaciones, como solicitudes de red o tareas que pueden no completarse
/// en el primer intento. Las políticas de reintentos son importantes para mejorar la resiliencia
/// de las aplicaciones al permitir que se reintenten operaciones fallidas con diferentes configuraciones.
/// </remarks>
public enum RetryPolicyDefault
{
    /// <summary>
    /// Representa una política de reintentos baja: 2 intentos, 1 segundo de espera.
    /// </summary>
    LowRetries,      
    
    /// <summary>
    /// Representa una política de reintentos media: 3 intentos, 2 segundos de espera.
    /// </summary>
    MediumRetries, 
    
    /// <summary>
    /// Representa una política de reintentos alta: 5 intentos, 5 segundos de espera.
    /// </summary>
    HighRetries,  
    
    /// <summary>
    /// Representa una política de reintentos muy alta: 7 intentos, 10 segundos de espera.
    /// </summary>
    VeryHighRetries,  
    
    /// <summary>
    /// Representa una política de reintentos extrema: 10 intentos, 15 segundos de espera.
    /// </summary>
    ExtremeRetries, 
    
    /// <summary>
    /// Representa una política de no reintentos: solo se intenta una vez.
    /// </summary>
    NoRetries          
}