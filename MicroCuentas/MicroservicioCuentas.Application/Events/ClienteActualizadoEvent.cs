namespace MicroservicioCuentas.Application.Events;

/// <summary>
/// Representa el evento generado cuando se actualiza la información
/// relevante de un cliente.
/// </summary>
/// <param name="ClienteId">Identificador del cliente.</param>
/// <param name="Nombre">Nombre actualizado del cliente.</param>
/// <param name="Estado">Estado actualizado del cliente.</param>
public record ClienteActualizadoEvent(
    int ClienteId,
    string Nombre,
    bool Estado);