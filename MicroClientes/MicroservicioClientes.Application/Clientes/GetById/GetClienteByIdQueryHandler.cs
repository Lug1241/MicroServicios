using MediatR;
using MicroservicioClientes.Domain.Clientes;

namespace MicroservicioClientes.Application.Clientes.GetById;

/// <summary>
/// Maneja la consulta de un cliente mediante su identificador.
/// </summary>
public sealed class GetClienteByIdQueryHandler
    : IRequestHandler<GetClienteByIdQuery, ClienteResponse>
{
    private readonly IClienteRepository _clienteRepository;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="GetClienteByIdQueryHandler"/>.
    /// </summary>
    /// <param name="clienteRepository">
    /// Repositorio utilizado para consultar los clientes.
    /// </param>
    public GetClienteByIdQueryHandler(
        IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    /// <summary>
    /// Obtiene un cliente mediante su identificador.
    /// </summary>
    /// <param name="query">Consulta que contiene el identificador del cliente.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Los datos del cliente encontrado.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Se produce cuando no existe un cliente con el identificador proporcionado.
    /// </exception>
    public async Task<ClienteResponse> Handle(
        GetClienteByIdQuery query,
        CancellationToken cancellationToken)
    {
        ClienteId clienteId = new ClienteId(query.Id);

        Cliente? cliente = await _clienteRepository.GetByIdAsync(
            clienteId,
            cancellationToken);

        if (cliente is null)
        {
            throw new KeyNotFoundException(
                $"No se encontró el cliente con Id {query.Id}.");
        }

        return new ClienteResponse(
            cliente.Id.Value,
            cliente.Nombre,
            cliente.Genero,
            cliente.Edad.Valor,
            cliente.Identificacion.Valor,
            cliente.Direccion,
            cliente.Telefono.Valor,
            cliente.Estado);
    }
}