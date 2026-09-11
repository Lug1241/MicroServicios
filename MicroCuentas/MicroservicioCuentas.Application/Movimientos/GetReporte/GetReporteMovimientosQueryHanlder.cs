using MediatR;
using MicroservicioCuentas.Domain.ClientesReferencia;
using MicroservicioCuentas.Domain.Cuentas;
using MicroservicioCuentas.Domain.Movimiento;

namespace MicroservicioCuentas.Application.Movimientos.GetReporte;

/// <summary>
/// Maneja la consulta del reporte de movimientos de un cliente.
/// </summary>
public class GetReporteMovimientosQueryHandler
    : IRequestHandler<
        GetReporteMovimientosQuery,
        List<ReporteMovimientoResponse>>
{
    private readonly ICuentaRepository _cuentaRepository;
    private readonly IMovimientoRepository _movimientoRepository;
    private readonly IClienteReferenciaRepository _clienteReferenciaRepository;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="GetReporteMovimientosQueryHandler"/>.
    /// </summary>
    /// <param name="cuentaRepository">
    /// Repositorio utilizado para consultar las cuentas.
    /// </param>
    /// <param name="movimientoRepository">
    /// Repositorio utilizado para consultar los movimientos.
    /// </param>
    /// <param name="clienteReferenciaRepository">
    /// Repositorio utilizado para consultar la referencia local del cliente.
    /// </param>
    public GetReporteMovimientosQueryHandler(
        ICuentaRepository cuentaRepository,
        IMovimientoRepository movimientoRepository,
        IClienteReferenciaRepository clienteReferenciaRepository)
    {
        _cuentaRepository = cuentaRepository;
        _movimientoRepository = movimientoRepository;
        _clienteReferenciaRepository = clienteReferenciaRepository;
    }

    /// <summary>
    /// Obtiene los movimientos de las cuentas asociadas a un cliente
    /// dentro del rango de fechas solicitado.
    /// </summary>
    /// <param name="request">Parámetros de la consulta.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Lista de movimientos que forman parte del reporte.</returns>
    public async Task<List<ReporteMovimientoResponse>> Handle(
        GetReporteMovimientosQuery request,
        CancellationToken cancellationToken)
    {
        List<Cuenta> cuentas =
            await _cuentaRepository.GetByClienteIdAsync(
                request.ClienteId,
                cancellationToken);

        if (cuentas.Count == 0)
        {
            return [];
        }

        ClienteReferencia? cliente =
            await _clienteReferenciaRepository.GetByIdAsync(
                request.ClienteId,
                cancellationToken);

        if (cliente is null)
        {
            throw new KeyNotFoundException(
                "El cliente no existe.");
        }

        List<CuentaId> cuentaIds = cuentas
            .Select(cuenta => cuenta.Id)
            .ToList();

        List<Movimiento> movimientos =
            await _movimientoRepository.GetByCuentasAndFechaAsync(
                cuentaIds,
                request.FechaInicio,
                request.FechaFin,
                cancellationToken);

        return movimientos
            .Select(movimiento =>
            {
                Cuenta cuenta = cuentas.First(
                    cuenta => cuenta.Id == movimiento.CuentaId);

                return new ReporteMovimientoResponse(
                    movimiento.Fecha.ToString("d/M/yyyy"),
                    cliente.Nombre,
                    cuenta.NumeroCuenta,
                    cuenta.TipoCuenta.ToString(),
                    cuenta.SaldoInicial.Valor,
                    cuenta.Estado,
                    movimiento.Valor,
                    movimiento.Saldo);
            })
            .ToList();
    }
}