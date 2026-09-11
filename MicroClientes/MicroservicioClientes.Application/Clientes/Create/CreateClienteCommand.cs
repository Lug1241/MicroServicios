using MediatR;
using MicroservicioClientes.Domain.Enums;

namespace MicroservicioClientes.Application.Clientes.Create;

/// <summary>
/// Representa la solicitud para crear un nuevo cliente.
/// </summary>
/// <param name="Nombre">Nombre del cliente.</param>
/// <param name="Genero">Género del cliente.</param>
/// <param name="Edad">Edad del cliente.</param>
/// <param name="Identificacion">Identificación del cliente.</param>
/// <param name="Direccion">Dirección del cliente.</param>
/// <param name="Telefono">Número de teléfono del cliente.</param>
/// <param name="Contraseña">Contraseña del cliente.</param>
public record CreateClienteCommand(
    string Nombre,
    Genero Genero,
    int Edad,
    string Identificacion,
    string Direccion,
    string Telefono,
    string Contraseña
) : IRequest<ClienteResponse>;