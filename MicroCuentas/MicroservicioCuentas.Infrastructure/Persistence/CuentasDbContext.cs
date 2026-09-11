using MediatR;
using Microsoft.EntityFrameworkCore;
using MicroservicioCuentas.Domain.ClientesReferencia;
using MicroservicioCuentas.Domain.Cuentas;
using MicroservicioCuentas.Domain.Movimiento;
using MicroservicioCuentas.Domain.Primitives;

namespace MicroservicioCuentas.Infrastructure.Persistence;

/// <summary>
/// Contexto de persistencia del microservicio de cuentas.
/// También implementa la unidad de trabajo utilizada por la aplicación.
/// </summary>
public sealed class CuentasDbContext : DbContext, IUnitOfWork
{
    private readonly IPublisher _publisher;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="CuentasDbContext"/>.
    /// </summary>
    /// <param name="options">Opciones de configuración del contexto.</param>
    /// <param name="publisher">
    /// Componente utilizado para publicar eventos de dominio.
    /// </param>
    public CuentasDbContext(
        DbContextOptions<CuentasDbContext> options,
        IPublisher publisher)
        : base(options)
    {
        _publisher = publisher;
    }

    /// <summary>
    /// Obtiene las cuentas almacenadas en la base de datos.
    /// </summary>
    public DbSet<Cuenta> Cuentas { get; set; }

    /// <summary>
    /// Obtiene los movimientos almacenados en la base de datos.
    /// </summary>
    public DbSet<Movimiento> Movimientos { get; set; }

    /// <summary>
    /// Obtiene las referencias locales de clientes almacenadas en la base de datos.
    /// </summary>
    public DbSet<ClienteReferencia> ClientesReferencia { get; set; }

    /// <summary>
    /// Configura el modelo de entidades utilizando las configuraciones
    /// definidas en el assembly de infraestructura.
    /// </summary>
    /// <param name="modelBuilder">Constructor del modelo de Entity Framework Core.</param>
    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CuentasDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Persiste los cambios pendientes y publica los eventos de dominio
    /// generados por las entidades modificadas.
    /// </summary>
    /// <param name="cancellationToken">
    /// Token utilizado para cancelar la operación.
    /// </param>
    /// <returns>El número de registros afectados.</returns>
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        List<AggregateRoot> domainEntities = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(entry => entry.Entity.GetDomainEvents().Any())
            .Select(entry => entry.Entity)
            .ToList();

        List<DomainEvent> domainEvents = domainEntities
            .SelectMany(entity => entity.GetDomainEvents())
            .ToList();

        int result = await base.SaveChangesAsync(
            cancellationToken);

        foreach (DomainEvent domainEvent in domainEvents)
        {
            await _publisher.Publish(
                domainEvent,
                cancellationToken);
        }

        foreach (AggregateRoot domainEntity in domainEntities)
        {
            domainEntity.ClearDomainEvents();
        }

        return result;
    }
}