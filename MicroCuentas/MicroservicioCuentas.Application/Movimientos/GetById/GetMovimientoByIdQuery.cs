using MediatR;

namespace MicroservicioCuentas.Application.Movimientos.GetById;

/// <summary>
/// Representa la solicitud para obtener un movimiento mediante su identificador.
/// </summary>
/// <param name="Id">Identificador del movimiento.</param>
public record GetMovimientoByIdQuery(int Id)
    : IRequest<MovimientoResponse>;