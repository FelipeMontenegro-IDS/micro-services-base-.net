namespace Shared.Enums;

public enum RetryPolicyDefaults
{
    LowRetries,         // Ejemplo: 2 intentos, 1 segundo de espera
    MediumRetries,      // Ejemplo: 3 intentos, 2 segundos de espera
    HighRetries,        // Ejemplo: 5 intentos, 5 segundos de espera
    VeryHighRetries,    // Ejemplo: 7 intentos, 10 segundos de espera
    ExtremeRetries,     // Ejemplo: 10 intentos, 15 segundos de espera
    NoRetries          // No reintentar, solo una vez
}