using MediatR;

namespace MicroservicioCuentas.Domain.Primitives;

public abstract record DomainEvent : INotification;
