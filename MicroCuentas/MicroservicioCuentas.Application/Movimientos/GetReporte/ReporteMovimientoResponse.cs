namespace MicroservicioCuentas.Application.Movimientos.GetReporte;

/// <summary>
/// Representa un registro del reporte de movimientos.
/// </summary>
/// <param name="Fecha">Fecha en la que se realizó el movimiento.</param>
/// <param name="Cliente">Nombre del cliente propietario de la cuenta.</param>
/// <param name="NumeroCuenta">Número de la cuenta.</param>
/// <param name="Tipo">Tipo de cuenta.</param>
/// <param name="SaldoInicial">Saldo inicial de la cuenta.</param>
/// <param name="Estado">Estado actual de la cuenta.</param>
/// <param name="Movimiento">Valor del movimiento realizado.</param>
/// <param name="SaldoDisponible">Saldo disponible después del movimiento.</param>
public record ReporteMovimientoResponse(
    string Fecha,
    string Cliente,
    int NumeroCuenta,
    string Tipo,
    decimal SaldoInicial,
    bool Estado,
    decimal Movimiento,
    decimal SaldoDisponible);