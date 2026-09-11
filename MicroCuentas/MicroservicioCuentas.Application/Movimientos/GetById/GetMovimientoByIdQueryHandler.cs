using MediatR;
using MicroservicioCuentas.Domain.Cuentas;
using MicroservicioCuentas.Domain.Enums;
using MicroservicioCuentas.Domain.Movimiento;

namespace MicroservicioCuentas.Application.Movimientos.GetById;

/// <summary>
/// Maneja la consulta de un movimiento mediante su identificador.
/// </summary>
public class GetMovimientoByIdQueryHandler
    : IRequestHandler<GetMovimientoByIdQuery, MovimientoResponse>
{
    private readonly IMovimientoRepository _movimientoRepository;
    private readonly ICuentaRepository _cuentaRepository;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="GetMovimientoByIdQueryHandler"/>.
    /// </summary>
    /// <param name="movimientoRepository">
    /// Repositorio utilizado para consultar movimientos.
    /// </param>
    /// <param name="cuentaRepository">
    /// Repositorio utilizado para consultar cuentas.
    /// </param>
    public GetMovimientoByIdQueryHandler(
        IMovimientoRepository movimientoRepository,
        ICuentaRepository cuentaRepository)
    {
        _movimientoRepository = movimientoRepository;
        _cuentaRepository = cuentaRepository;
    }

    /// <summary>
    /// Obtiene un movimiento y la información de la cuenta asociada.
    /// </summary>
    /// <param name="request">Parámetros de la consulta.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Información del movimiento y de la cuenta asociada.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Se produce cuando el movimiento o la cuenta asociada no existen.
    /// </exception>
    public async Task<MovimientoResponse> Handle(
        GetMovimientoByIdQuery request,
        CancellationToken cancellationToken)
    {
        Movimiento? movimiento =
            await _movimientoRepository.GetByIdAsync(
                new MovimientoId(request.Id),
                cancellationToken);

        if (movimiento is null)
        {
            throw new KeyNotFoundException(
                "El movimiento no existe.");
        }

        Cuenta? cuenta =
            await _cuentaRepository.GetByIdAsync(
                movimiento.CuentaId,
                cancellationToken);

        if (cuenta is null)
        {
            throw new KeyNotFoundException(
                "La cuenta asociada al movimiento no existe.");
        }

        TipoMovimiento tipoMovimiento =
            movimiento.TipoMovimiento;

        decimal valor =
            Math.Abs(movimiento.Valor);

        string descripcionMovimiento =
            $"{tipoMovimiento} de {valor}";

        return new MovimientoResponse(
            cuenta.Id.Value,
            cuenta.NumeroCuenta,
            cuenta.TipoCuenta,
            cuenta.SaldoInicial.Valor,
            cuenta.Estado,
            descripcionMovimiento);
    }
}