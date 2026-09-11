using MediatR;
using MicroservicioCuentas.Domain.Cuentas;
using MicroservicioCuentas.Domain.Primitives;

namespace MicroservicioCuentas.Application.Cuentas.Update;

/// <summary>
/// Maneja la actualización del estado de una cuenta.
/// </summary>
public class UpdateCuentaCommandHandler
    : IRequestHandler<UpdateCuentaCommand>
{
    private readonly ICuentaRepository _cuentaRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="UpdateCuentaCommandHandler"/>.
    /// </summary>
    /// <param name="cuentaRepository">
    /// Repositorio utilizado para consultar y actualizar cuentas.
    /// </param>
    /// <param name="unitOfWork">
    /// Unidad de trabajo utilizada para persistir los cambios.
    /// </param>
    public UpdateCuentaCommandHandler(
        ICuentaRepository cuentaRepository,
        IUnitOfWork unitOfWork)
    {
        _cuentaRepository = cuentaRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Actualiza el estado de una cuenta existente.
    /// </summary>
    /// <param name="request">Datos utilizados para actualizar la cuenta.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <exception cref="KeyNotFoundException">
    /// Se produce cuando la cuenta no existe.
    /// </exception>
    public async Task Handle(
        UpdateCuentaCommand request,
        CancellationToken cancellationToken)
    {
        Cuenta? cuenta =
            await _cuentaRepository.GetByIdAsync(
                new CuentaId(request.Id),
                cancellationToken);

        if (cuenta is null)
        {
            throw new KeyNotFoundException(
                "La cuenta no existe.");
        }

        if (request.Estado)
        {
            cuenta.Activar();
        }
        else
        {
            cuenta.Inactivar();
        }

        _cuentaRepository.Update(cuenta);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}