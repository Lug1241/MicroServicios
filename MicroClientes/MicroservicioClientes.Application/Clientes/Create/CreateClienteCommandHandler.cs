using MediatR;
using MicroservicioClientes.Application.Events;
using MicroservicioClientes.Domain;
using MicroservicioClientes.Domain.Clientes;
using MicroservicioClientes.Domain.Primitives;
using MicroservicioClientes.Domain.ValueObjects;

namespace MicroservicioClientes.Application.Clientes.Create;

/// <summary>
/// Maneja la creación de un nuevo cliente.
/// </summary>
public sealed class CreateClienteCommandHandler
    : IRequestHandler<CreateClienteCommand, ClienteResponse>
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisher _eventPublisher;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="CreateClienteCommandHandler"/>.
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
    public CreateClienteCommandHandler(
        IClienteRepository clienteRepository,
        IUnitOfWork unitOfWork,
        IEventPublisher eventPublisher)
    {
        _clienteRepository = clienteRepository;
        _unitOfWork = unitOfWork;
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// Crea un nuevo cliente, persiste sus datos y publica el evento de integración.
    /// </summary>
    /// <param name="command">Comando con los datos del cliente.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Los datos del cliente creado.</returns>
    public async Task<ClienteResponse> Handle(
        CreateClienteCommand command,
        CancellationToken cancellationToken)
    {
        Identificacion identificacion = Identificacion.Create(
            command.Identificacion);

        Cliente? clienteExistente =
            await _clienteRepository.GetByIdentificacionAsync(
                identificacion,
                cancellationToken);

        if (clienteExistente is not null)
        {
            throw new DomainException(
                "Ya existe un cliente con esa identificación.");
        }

        Telefono telefono = Telefono.Create(
            command.Telefono);

        Edad edad = Edad.Create(
            command.Edad);

        Cliente cliente = Cliente.Create(
            command.Nombre,
            command.Genero,
            edad,
            identificacion,
            command.Direccion,
            telefono,
            command.Contraseña);

        _clienteRepository.Add(cliente);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        await _eventPublisher.PublishAsync(
            new ClienteActualizadoEvent(
                cliente.Id.Value,
                cliente.Nombre,
                cliente.Estado),
            cancellationToken);

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