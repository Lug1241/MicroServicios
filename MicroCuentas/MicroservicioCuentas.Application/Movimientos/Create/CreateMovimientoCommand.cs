using MediatR;
using MicroservicioCuentas.Domain.Enums;

namespace MicroservicioCuentas.Application.Movimientos.Create;

/// <summary>
/// Representa la solicitud para crear un nuevo movimiento en una cuenta.
/// </summary>
/// <param name="CuentaId">Identificador de la cuenta.</param>
/// <param name="TipoMovimiento">Tipo de movimiento que se desea registrar.</param>
/// <param name="Valor">Valor del movimiento.</param>
public record CreateMovimientoCommand(
    int CuentaId,
    TipoMovimiento TipoMovimiento,
    decimal Valor)
    : IRequest<MovimientoResponse>;