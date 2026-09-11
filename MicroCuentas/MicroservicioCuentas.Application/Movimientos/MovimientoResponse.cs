using MicroservicioCuentas.Domain.Enums;

namespace MicroservicioCuentas.Application.Movimientos;

/// <summary>
/// Representa la información de una cuenta junto con su movimiento.
/// </summary>
/// <param name="Id">Identificador de la cuenta.</param>
/// <param name="NumeroCuenta">Número de cuenta.</param>
/// <param name="TipoCuenta">Tipo de cuenta.</param>
/// <param name="SaldoInicial">Saldo inicial de la cuenta.</param>
/// <param name="Estado">Estado actual de la cuenta.</param>
/// <param name="Movimiento">Descripción del movimiento realizado.</param>
public record MovimientoResponse(
    int Id,
    int NumeroCuenta,
    TipoCuenta TipoCuenta,
    decimal SaldoInicial,
    bool Estado,
    string Movimiento);