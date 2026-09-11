using MediatR;

namespace MicroservicioClientes.Application.Clientes.GetById;

/// <summary>
/// Representa la consulta para obtener un cliente mediante su identificador.
/// </summary>
/// <param name="Id">Identificador del cliente que se desea consultar.</param>
public record GetClienteByIdQuery(int Id) : IRequest<ClienteResponse>;