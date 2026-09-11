using MediatR;
using MicroservicioClientes.Domain.Clientes;
using MicroservicioClientes.Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace MicroservicioClientes.Infrastructure.Persistence;

/// <summary>
/// Representa el contexto de persistencia de los clientes.
/// </summary>
public sealed class ClientesDbContext : DbContext, IUnitOfWork
{
    private readonly IPublisher _publisher;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="ClientesDbContext"/>.
    /// </summary>
    /// <param name="options">Opciones de configuración del contexto.</param>
    /// <param name="publisher">Componente utilizado para publicar eventos de dominio.</param>
    public ClientesDbContext(
        DbContextOptions<ClientesDbContext> options,
        IPublisher publisher)
        : base(options)
    {
        _publisher = publisher;
    }

    /// <summary>
    /// Obtiene o establece el conjunto de personas.
    /// </summary>
    public DbSet<Persona> Personas { get; set; }

    /// <summary>
    /// Obtiene o establece el conjunto de clientes.
    /// </summary>
    public DbSet<Cliente> Clientes { get; set; }

    /// <summary>
    /// Configura el modelo de entidades utilizado por el contexto.
    /// </summary>
    /// <param name="modelBuilder">Constructor utilizado para configurar el modelo.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ClientesDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Persiste los cambios y publica los eventos de dominio generados por los agregados.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>El número de registros afectados.</returns>
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        List<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AggregateRoot>> domainEntities =
            ChangeTracker
                .Entries<AggregateRoot>()
                .Where(entry => entry.Entity.GetDomainEvents().Any())
                .ToList();

        List<DomainEvent> domainEvents = domainEntities
            .SelectMany(entry => entry.Entity.GetDomainEvents())
            .ToList();

        int result = await base.SaveChangesAsync(cancellationToken);

        foreach (DomainEvent domainEvent in domainEvents)
        {
            await _publisher.Publish(
                domainEvent,
                cancellationToken);
        }

        domainEntities.ForEach(
            entity => entity.Entity.ClearDomainEvents());

        return result;
    }
}