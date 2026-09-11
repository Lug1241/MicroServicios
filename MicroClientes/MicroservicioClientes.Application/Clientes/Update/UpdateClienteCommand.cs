using MediatR;
using MicroservicioClientes.Domain.Enums;

namespace MicroservicioClientes.Application.Clientes.Update;

/// <summary>
/// Representa la solicitud para actualizar un cliente existente.
/// </summary>
/// <param name="Id">Identificador del cliente que se desea actualizar.</param>
/// <param name="Nombre">Nuevo nombre del cliente.</param>
/// <param name="Genero">Nuevo género del cliente.</param>
/// <param name="Edad">Nueva edad del cliente.</param>
/// <param name="Identificacion">Nueva identificación del cliente.</param>
/// <param name="Direccion">Nueva dirección del cliente.</param>
/// <param name="Telefono">Nuevo número de teléfono del cliente.</param>
/// <param name="Contraseña">Nueva contraseña del cliente.</param>
public record UpdateClienteCommand(
    int Id,
    string Nombre,
    Genero Genero,
    int Edad,
    string Identificacion,
    string Direccion,
    string Telefono,
    string Contraseña
) : IRequest;