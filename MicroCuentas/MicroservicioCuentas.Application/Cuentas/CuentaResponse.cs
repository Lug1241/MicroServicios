using MicroservicioCuentas.Domain.Enums;

namespace MicroservicioCuentas.Application.Cuentas;

/// <summary>
/// Representa la información de una cuenta.
/// </summary>
/// <param name="Id">Identificador de la cuenta.</param>
/// <param name="NumeroCuenta">Número de cuenta.</param>
/// <param name="TipoCuenta">Tipo de cuenta.</param>
/// <param name="SaldoInicial">Saldo inicial de la cuenta.</param>
/// <param name="Estado">Estado actual de la cuenta.</param>
/// <param name="Cliente">Nombre del cliente propietario de la cuenta.</param>
public record CuentaResponse(
    int Id,
    int NumeroCuenta,
    TipoCuenta TipoCuenta,
    decimal SaldoInicial,
    bool Estado,
    string Cliente);