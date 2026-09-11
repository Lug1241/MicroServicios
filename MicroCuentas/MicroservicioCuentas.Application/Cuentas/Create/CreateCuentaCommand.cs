using MediatR;
using MicroservicioCuentas.Domain.Enums;

namespace MicroservicioCuentas.Application.Cuentas.Create;

/// <summary>
/// Representa la solicitud para crear una nueva cuenta.
/// </summary>
/// <param name="ClienteId">Identificador del cliente propietario de la cuenta.</param>
/// <param name="TipoCuenta">Tipo de cuenta que se desea crear.</param>
/// <param name="SaldoInicial">Saldo inicial de la cuenta.</param>
/// <param name="NumeroCuenta">Número de la cuenta.</param>
public record CreateCuentaCommand(
    int ClienteId,
    TipoCuenta TipoCuenta,
    decimal SaldoInicial,
    int NumeroCuenta)
    : IRequest<CuentaResponse>;