using Microsoft.EntityFrameworkCore;
using MicroservicioCuentas.Domain.Cuentas;

namespace MicroservicioCuentas.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementa las operaciones de persistencia relacionadas con las cuentas.
/// </summary>
public class CuentaRepository : ICuentaRepository
{
    private readonly CuentasDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="CuentaRepository"/>.
    /// </summary>
    /// <param name="dbContext">Contexto de persistencia de cuentas.</param>
    public CuentaRepository(CuentasDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene una cuenta mediante su identificador.
    /// </summary>
    /// <param name="id">Identificador de la cuenta.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>
    /// La cuenta encontrada o <see langword="null"/> si no existe.
    /// </returns>
    public async Task<Cuenta?> GetByIdAsync(
        CuentaId id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Cuentas
            .FirstOrDefaultAsync(
                cuenta => cuenta.Id == id,
                cancellationToken);
    }

    /// <summary>
    /// Obtiene las cuentas asociadas a un cliente.
    /// </summary>
    /// <param name="clienteId">Identificador del cliente.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Lista de cuentas asociadas al cliente.</returns>
    public async Task<List<Cuenta>> GetByClienteIdAsync(
        int clienteId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Cuentas
            .Where(cuenta => cuenta.ClienteId == clienteId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Obtiene una cuenta mediante su número de cuenta.
    /// </summary>
    /// <param name="cuenta">Número de cuenta que se desea consultar.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>
    /// La cuenta encontrada o <see langword="null"/> si no existe.
    /// </returns>
    public async Task<Cuenta?> GetByNumeroCuentaAsync(
        int cuenta,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Cuentas
            .FirstOrDefaultAsync(
                cuentaEntity => cuentaEntity.NumeroCuenta == cuenta,
                cancellationToken);
    }

    /// <summary>
    /// Agrega una nueva cuenta al contexto de persistencia.
    /// </summary>
    /// <param name="cuenta">Cuenta que se desea agregar.</param>
    public void Add(Cuenta cuenta)
    {
        _dbContext.Cuentas.Add(cuenta);
    }

    /// <summary>
    /// Marca una cuenta existente para su actualización.
    /// </summary>
    /// <param name="cuenta">Cuenta que se desea actualizar.</param>
    public void Update(Cuenta cuenta)
    {
        _dbContext.Cuentas.Update(cuenta);
    }
}