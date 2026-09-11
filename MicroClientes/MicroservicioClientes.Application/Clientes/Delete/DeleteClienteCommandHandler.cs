using MediatR;
using MicroservicioClientes.Application.Events;
using MicroservicioClientes.Domain.Clientes;
using MicroservicioClientes.Domain.Primitives;

namespace MicroservicioClientes.Application.Clientes.Delete;

/// <summary>
/// Maneja la inactivación de un cliente.
/// </summary>
public sealed class DeleteClienteCommandHandler
    : IRequestHandler<DeleteClienteCommand>
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisher _eventPublisher;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="DeleteClienteCommandHandler"/>.
    /// </summary>
    /// <param name="clienteRepository">
    /// Repositorio utilizado para consultar y persistir clientes.
    /// </param>
    /// <param name="unitOfWork">
    /// Unidad de trabajo utilizada para persistir los cambios.
    /// </param>
    /// <param name="eventPublisher">
    /// Publicador utilizado para enviar eventos de integración.
    /// </param>
    public DeleteClienteCommandHandler(
        IClienteRepository clienteRepository,
        IUnitOfWork unitOfWork,
        IEventPublisher eventPublisher)
    {
        _clienteRepository = clienteRepository;
        _unitOfWork = unitOfWork;
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// Inactiva un cliente existente y publica el evento correspondiente.
    /// </summary>
    /// <param name="command">Comando con el identificador del cliente.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    public async Task Handle(
        DeleteClienteCommand command,
        CancellationToken cancellationToken)
    {
        ClienteId clienteId = new ClienteId(command.Id);

        Cliente? cliente = await _clienteRepository.GetByIdAsync(
            clienteId,
            cancellationToken);

        if (cliente is null)
        {
            throw new KeyNotFoundException(
                $"No se encontró el cliente con Id {command.Id}.");
        }

        cliente.Inactivar();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        await _eventPublisher.PublishAsync(
            new ClienteActualizadoEvent(
                cliente.Id.Value,
                cliente.Nombre,
                cliente.Estado),
            cancellationToken);
    }
}