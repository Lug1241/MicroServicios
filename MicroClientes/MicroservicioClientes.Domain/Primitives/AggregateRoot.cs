namespace MicroservicioClientes.Domain.Primitives;

/// <summary>
/// Representa la raíz de un agregado y administra sus eventos de dominio.
/// </summary>
public abstract class AggregateRoot
{
    private readonly List<DomainEvent> _domainEvents = new();

    /// <summary>
    /// Obtiene los eventos de dominio registrados en el agregado.
    /// </summary>
    public IReadOnlyCollection<DomainEvent> GetDomainEvents() =>
        _domainEvents.ToList();

    /// <summary>
    /// Elimina todos los eventos de dominio registrados en el agregado.
    /// </summary>
    public void ClearDomainEvents() =>
        _domainEvents.Clear();

    /// <summary>
    /// Registra un evento de dominio en el agregado.
    /// </summary>
    /// <param name="domainEvent">Evento de dominio que se desea registrar.</param>
    protected void RaiseDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}