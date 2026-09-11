using MediatR;

namespace MicroservicioCuentas.Application.Movimientos.GetReporte;

/// <summary>
/// Representa la solicitud para obtener el reporte de movimientos de un cliente
/// dentro de un rango de fechas.
/// </summary>
/// <param name="ClienteId">Identificador del cliente.</param>
/// <param name="FechaInicio">Fecha inicial del rango de consulta.</param>
/// <param name="FechaFin">Fecha final del rango de consulta.</param>
public record GetReporteMovimientosQuery(
    int ClienteId,
    DateTime FechaInicio,
    DateTime FechaFin)
    : IRequest<List<ReporteMovimientoResponse>>;