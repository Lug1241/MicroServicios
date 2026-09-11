using MediatR;

namespace MicroservicioClientes.Domain.Primitives;

public abstract record DomainEvent : INotification;