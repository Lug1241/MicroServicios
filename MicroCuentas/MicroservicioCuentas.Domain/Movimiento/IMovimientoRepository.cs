using MicroservicioCuentas.Domain.Cuentas;

namespace MicroservicioCuentas.Domain.Movimiento;

/// <summary>
/// Define las operaciones de persistencia relacionadas con los movimientos.
/// </summary>
public interface IMovimientoRepository
{
    /// <summary>
    /// Obtiene el último movimiento realizado en una cuenta.
    /// </summary>
    /// <param name="cuentaId">Identificador de la cuenta.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>El último movimiento registrado para la cuenta.</returns>
    Task<Movimiento?> GetUltimoMovimientoAsync(
        CuentaId cuentaId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un movimiento mediante su identificador.
    /// </summary>
    /// <param name="id">Identificador del movimiento.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>El movimiento correspondiente al identificador proporcionado.</returns>
    Task<Movimiento?> GetByIdAsync(
        MovimientoId id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene los movimientos de un conjunto de cuentas dentro de un rango de fechas.
    /// </summary>
    /// <param name="cuentaIds">Identificadores de las cuentas que se desean consultar.</param>
    /// <param name="fechaInicio">Fecha inicial del rango de consulta.</param>
    /// <param name="fechaFin">Fecha final del rango de consulta.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Lista de movimientos encontrados.</returns>
    Task<List<Movimiento>> GetByCuentasAndFechaAsync(
        List<CuentaId> cuentaIds,
        DateTime fechaInicio,
        DateTime fechaFin,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Agrega un nuevo movimiento.
    /// </summary>
    /// <param name="movimiento">Movimiento que se desea agregar.</param>
    void Add(Movimiento movimiento);
}