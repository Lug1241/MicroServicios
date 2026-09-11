using Microsoft.EntityFrameworkCore;
using MicroservicioCuentas.Domain.Cuentas;
using MicroservicioCuentas.Domain.Movimiento;

namespace MicroservicioCuentas.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementa las operaciones de persistencia relacionadas con los movimientos.
/// </summary>
public class MovimientoRepository : IMovimientoRepository
{
    private readonly CuentasDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="MovimientoRepository"/>.
    /// </summary>
    /// <param name="dbContext">Contexto de persistencia de cuentas.</param>
    public MovimientoRepository(CuentasDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Obtiene el último movimiento realizado en una cuenta.
    /// </summary>
    /// <param name="cuentaId">Identificador de la cuenta.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>
    /// El último movimiento de la cuenta o <see langword="null"/> si no existe.
    /// </returns>
    public async Task<Movimiento?> GetUltimoMovimientoAsync(
        CuentaId cuentaId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Movimientos
            .Where(movimiento => movimiento.CuentaId == cuentaId)
            .OrderByDescending(movimiento => movimiento.Fecha)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Obtiene un movimiento mediante su identificador.
    /// </summary>
    /// <param name="id">Identificador del movimiento.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>
    /// El movimiento encontrado o <see langword="null"/> si no existe.
    /// </returns>
    public async Task<Movimiento?> GetByIdAsync(
        MovimientoId id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Movimientos
            .FirstOrDefaultAsync(
                movimiento => movimiento.Id == id,
                cancellationToken);
    }

    /// <summary>
    /// Agrega un nuevo movimiento al contexto de persistencia.
    /// </summary>
    /// <param name="movimiento">Movimiento que se desea agregar.</param>
    public void Add(Movimiento movimiento)
    {
        _dbContext.Movimientos.Add(movimiento);
    }

    /// <summary>
    /// Obtiene los movimientos de un conjunto de cuentas dentro de un rango de fechas.
    /// </summary>
    /// <param name="cuentaIds">Identificadores de las cuentas.</param>
    /// <param name="fechaInicio">Fecha inicial del rango.</param>
    /// <param name="fechaFin">Fecha final del rango.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Lista de movimientos encontrados.</returns>
    public async Task<List<Movimiento>> GetByCuentasAndFechaAsync(
        List<CuentaId> cuentaIds,
        DateTime fechaInicio,
        DateTime fechaFin,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Movimientos
            .Where(movimiento =>
                cuentaIds.Contains(movimiento.CuentaId) &&
                movimiento.Fecha >= fechaInicio &&
                movimiento.Fecha < fechaFin.AddDays(1))
            .OrderBy(movimiento => movimiento.Fecha)
            .ToListAsync(cancellationToken);
    }
}