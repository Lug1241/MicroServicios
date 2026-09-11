using MicroservicioClientes.Domain.Enums;

namespace MicroservicioClientes.Application.Clientes;

/// <summary>
/// Representa los datos de un cliente devueltos por la aplicación.
/// </summary>
/// <param name="Id">Identificador del cliente.</param>
/// <param name="Nombre">Nombre del cliente.</param>
/// <param name="Genero">Género del cliente.</param>
/// <param name="Edad">Edad del cliente.</param>
/// <param name="Identificacion">Identificación del cliente.</param>
/// <param name="Direccion">Dirección del cliente.</param>
/// <param name="Telefono">Número de teléfono del cliente.</param>
/// <param name="Estado">Indica si el cliente se encuentra activo.</param>
public record ClienteResponse(
    int Id,
    string Nombre,
    Genero Genero,
    int Edad,
    string Identificacion,
    string Direccion,
    string Telefono,
    bool Estado
);