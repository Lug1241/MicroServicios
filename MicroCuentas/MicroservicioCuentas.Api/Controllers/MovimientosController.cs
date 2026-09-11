using MediatR;
using MicroservicioCuentas.Application.Movimientos;
using MicroservicioCuentas.Application.Movimientos.Create;
using MicroservicioCuentas.Application.Movimientos.GetById;
using MicroservicioCuentas.Application.Movimientos.GetReporte;
using MicroservicioCuentas.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicioCuentas.Api.Controllers;

/// <summary>
/// Expone los endpoints HTTP relacionados con los movimientos de las cuentas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MovimientosController : ControllerBase
{
    private readonly ISender _sender;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="MovimientosController"/>.
    /// </summary>
    /// <param name="sender">
    /// Componente utilizado para enviar solicitudes a la capa de aplicación.
    /// </param>
    public MovimientosController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Registra un nuevo movimiento en una cuenta.
    /// </summary>
    /// <param name="command">
    /// Datos necesarios para crear el movimiento.
    /// </param>
    /// <param name="cancellationToken">
    /// Token para cancelar la operación.
    /// </param>
    /// <returns>
    /// Información del movimiento creado.
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateMovimientoCommand command,
        CancellationToken cancellationToken)
    {
        MovimientoResponse response =
            await _sender.Send(
                command,
                cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Obtiene un movimiento mediante su identificador.
    /// </summary>
    /// <param name="id">Identificador del movimiento.</param>
    /// <param name="cancellationToken">
    /// Token para cancelar la operación.
    /// </param>
    /// <returns>
    /// Información del movimiento consultado.
    /// </returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        GetMovimientoByIdQuery query =
            new GetMovimientoByIdQuery(id);

        MovimientoResponse response =
            await _sender.Send(
                query,
                cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Obtiene el reporte de movimientos de un cliente dentro de un rango de fechas.
    /// </summary>
    /// <param name="Fecha">
    /// Rango de fechas en formato dd/MM/yyyy-dd/MM/yyyy.
    /// </param>
    /// <param name="cliente">
    /// Identificador del cliente.
    /// </param>
    /// <param name="cancellationToken">
    /// Token para cancelar la operación.
    /// </param>
    /// <returns>
    /// Lista de movimientos correspondientes al cliente y rango indicado.
    /// </returns>
    [HttpGet("reportes")]
    public async Task<IActionResult> GetReporte(
        [FromQuery] string Fecha,
        [FromQuery] int cliente,
        CancellationToken cancellationToken)
    {
        RangoFechas rangoFechas =
            RangoFechas.Create(Fecha);

        GetReporteMovimientosQuery query =
            new GetReporteMovimientosQuery(
                cliente,
                rangoFechas.FechaInicio,
                rangoFechas.FechaFin);

        List<ReporteMovimientoResponse> response =
            await _sender.Send(
                query,
                cancellationToken);

        return Ok(response);
    }
}