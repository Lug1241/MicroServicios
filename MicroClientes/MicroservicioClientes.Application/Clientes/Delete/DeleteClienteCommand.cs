using MediatR;

namespace MicroservicioClientes.Application.Clientes.Delete;

/// <summary>
/// Representa la solicitud para eliminar un cliente.
/// </summary>
/// <param name="Id">Identificador del cliente que se desea eliminar.</param>
public record DeleteClienteCommand(int Id) : IRequest;