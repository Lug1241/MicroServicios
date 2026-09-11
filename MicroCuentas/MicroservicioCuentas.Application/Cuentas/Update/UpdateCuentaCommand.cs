using MediatR;

namespace MicroservicioCuentas.Application.Cuentas.Update;

/// <summary>
/// Representa la solicitud para actualizar el estado de una cuenta.
/// </summary>
/// <param name="Id">Identificador de la cuenta.</param>
/// <param name="Estado">Nuevo estado de la cuenta.</param>
public record UpdateCuentaCommand(
    int Id,
    bool Estado)
    : IRequest;