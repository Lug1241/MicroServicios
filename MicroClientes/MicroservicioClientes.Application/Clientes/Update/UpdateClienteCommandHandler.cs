using MediatR;
using MicroservicioClientes.Application.Events;
using MicroservicioClientes.Domain.Clientes;
using MicroservicioClientes.Domain.Primitives;
using MicroservicioClientes.Domain.ValueObjects;

namespace MicroservicioClientes.Application.Clientes.Update;

/// <summary>
/// Maneja la actualización de un cliente existente.
/// </summary>
public sealed class UpdateClienteCommandHandler
    : IRequestHandler<UpdateClienteCommand>
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisher _eventPublisher;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="UpdateClienteCommandHandler"/>.
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
    public UpdateClienteCommandHandler(
        IClienteRepository clienteRepository,
        IUnitOfWork unitOfWork,
        IEventPublisher eventPublisher)
    {
        _clienteRepository = clienteRepository;
        _unitOfWork = unitOfWork;
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// Actualiza un cliente existente, persiste los cambios y publica
    /// el evento de integración correspondiente.
    /// </summary>
    /// <param name="command">Comando con los datos actualizados del cliente.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <exception cref="KeyNotFoundException">
    /// Se produce cuando no existe un cliente con el identificador proporcionado.
    /// </exception>
    public async Task Handle(
        UpdateClienteCommand command,
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

        Identificacion identificacion = Identificacion.Create(
            command.Identificacion);

        Telefono telefono = Telefono.Create(
            command.Telefono);

        Edad edad = Edad.Create(
            command.Edad);

        cliente.Update(
            command.Nombre,
            command.Genero,
            edad,
            identificacion,
            command.Direccion,
            telefono,
            command.Contraseña);

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