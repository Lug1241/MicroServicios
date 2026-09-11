namespace MicroservicioCuentas.Domain.Enums;

/// <summary>
/// Define los tipos de movimiento disponibles para una cuenta.
/// </summary>
public enum TipoMovimiento
{
    /// <summary>
    /// Movimiento correspondiente a un depósito.
    /// </summary>
    Deposito,

    /// <summary>
    /// Movimiento correspondiente a un retiro.
    /// </summary>
    Retiro
}