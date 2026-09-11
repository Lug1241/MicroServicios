namespace MicroservicioClientes.Application.Events;

/// <summary>
/// Representa el evento de integración generado cuando se actualiza un cliente.
/// </summary>
/// <param name="ClienteId">Identificador del cliente.</param>
/// <param name="Nombre">Nombre actual del cliente.</param>
/// <param name="Estado">Estado actual del cliente.</param>
public record ClienteActualizadoEvent(
    int ClienteId,
    string Nombre,
    bool Estado);