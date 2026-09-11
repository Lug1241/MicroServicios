using MediatR;

namespace MicroservicioCuentas.Application.Cuentas.GetById;

/// <summary>
/// Representa la solicitud para obtener una cuenta mediante su identificador.
/// </summary>
/// <param name="Id">Identificador de la cuenta.</param>
public record GetCuentaByIdQuery(int Id)
    : IRequest<CuentaResponse>;