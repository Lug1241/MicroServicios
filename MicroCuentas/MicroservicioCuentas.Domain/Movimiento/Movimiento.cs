using MicroservicioCuentas.Domain.Cuentas;
using MicroservicioCuentas.Domain.Enums;
using MicroservicioCuentas.Domain.Primitives;

namespace MicroservicioCuentas.Domain.Movimiento;

/// <summary>
/// Representa un movimiento realizado sobre una cuenta.
/// </summary>
public class Movimiento : AggregateRoot
{
    /// <summary>
    /// Obtiene el identificador del movimiento.
    /// </summary>
    public MovimientoId Id { get; private set; }

    /// <summary>
    /// Obtiene la fecha en la que se realizó el movimiento.
    /// </summary>
    public DateTime Fecha { get; private set; }

    /// <summary>
    /// Obtiene el tipo de movimiento realizado.
    /// </summary>
    public TipoMovimiento TipoMovimiento { get; private set; }

    /// <summary>
    /// Obtiene el valor del movimiento.
    /// </summary>
    public decimal Valor { get; private set; }

    /// <summary>
    /// Obtiene el saldo de la cuenta después del movimiento.
    /// </summary>
    public decimal Saldo { get; private set; }

    /// <summary>
    /// Obtiene el identificador de la cuenta asociada al movimiento.
    /// </summary>
    public CuentaId CuentaId { get; private set; }

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="Movimiento"/>.
    /// </summary>
    /// <param name="fecha">Fecha del movimiento.</param>
    /// <param name="tipoMovimiento">Tipo de movimiento realizado.</param>
    /// <param name="valor">Valor del movimiento.</param>
    /// <param name="saldo">Saldo de la cuenta después del movimiento.</param>
    /// <param name="cuentaId">Identificador de la cuenta asociada.</param>
    public Movimiento(
        DateTime fecha,
        TipoMovimiento tipoMovimiento,
        decimal valor,
        decimal saldo,
        CuentaId cuentaId)
    {
        Fecha = fecha;
        TipoMovimiento = tipoMovimiento;
        Valor = valor;
        Saldo = saldo;
        CuentaId = cuentaId;
    }

    /// <summary>
    /// Constructor utilizado por Entity Framework Core.
    /// </summary>
    private Movimiento()
    {
    }

    /// <summary>
    /// Crea una nueva instancia de <see cref="Movimiento"/>.
    /// </summary>
    /// <param name="fecha">Fecha del movimiento.</param>
    /// <param name="tipoMovimiento">Tipo de movimiento realizado.</param>
    /// <param name="valor">Valor del movimiento.</param>
    /// <param name="saldo">Saldo de la cuenta después del movimiento.</param>
    /// <param name="cuentaId">Identificador de la cuenta asociada.</param>
    /// <returns>Una nueva instancia de <see cref="Movimiento"/>.</returns>
    public static Movimiento Create(
        DateTime fecha,
        TipoMovimiento tipoMovimiento,
        decimal valor,
        decimal saldo,
        CuentaId cuentaId)
    {
        Movimiento movimiento = new Movimiento(
            fecha,
            tipoMovimiento,
            valor,
            saldo,
            cuentaId);

        return movimiento;
    }
}