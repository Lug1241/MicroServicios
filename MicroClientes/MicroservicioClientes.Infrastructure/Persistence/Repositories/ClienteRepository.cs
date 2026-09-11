using MicroservicioClientes.Domain.Clientes;
using MicroservicioClientes.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace MicroservicioClientes.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementa las operaciones de persistencia relacionadas con los clientes.
/// </summary>
public sealed class ClienteRepository : IClienteRepository
{
    private readonly ClientesDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="ClienteRepository"/>.
    /// </summary>
    /// <param name="dbContext">Contexto de base de datos de clientes.</param>
    public ClienteRepository(ClientesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene un cliente mediante su identificador.
    /// </summary>
    /// <param name="id">Identificador del cliente.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>El cliente encontrado o <c>null</c> si no existe.</returns>
    public async Task<Cliente?> GetByIdAsync(
        ClienteId id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Clientes
            .FirstOrDefaultAsync(
                cliente => cliente.Id == id,
                cancellationToken);
    }

    /// <summary>
    /// Obtiene un cliente mediante su identificación.
    /// </summary>
    /// <param name="identificacion">Identificación del cliente.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>El cliente encontrado o <c>null</c> si no existe.</returns>
    public async Task<Cliente?> GetByIdentificacionAsync(
        Identificacion identificacion,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Clientes
            .FirstOrDefaultAsync(
                cliente => cliente.Identificacion.Equals(identificacion),
                cancellationToken);
    }

    /// <summary>
    /// Agrega un nuevo cliente al contexto de persistencia.
    /// </summary>
    /// <param name="cliente">Cliente que se desea agregar.</param>
    public void Add(Cliente cliente)
    {
        _dbContext.Clientes.Add(cliente);
    }

    /// <summary>
    /// Marca un cliente existente para actualizarlo.
    /// </summary>
    /// <param name="cliente">Cliente que se desea actualizar.</param>
    public void Update(Cliente cliente)
    {
        _dbContext.Clientes.Update(cliente);
    }

    /// <summary>
    /// Marca un cliente para eliminarlo.
    /// </summary>
    /// <param name="cliente">Cliente que se desea eliminar.</param>
    public void Delete(Cliente cliente)
    {
        _dbContext.Clientes.Remove(cliente);
    }
}