using MediatR;
using MicroservicioCuentas.Domain;
using MicroservicioCuentas.Domain.ClientesReferencia;
using MicroservicioCuentas.Domain.Cuentas;
using MicroservicioCuentas.Domain.Primitives;
using MicroservicioCuentas.Domain.ValueObjets;

namespace MicroservicioCuentas.Application.Cuentas.Create;

/// <summary>
/// Maneja la creación de una nueva cuenta.
/// </summary>
public class CreateCuentaCommandHandler
    : IRequestHandler<CreateCuentaCommand, CuentaResponse>
{
    private readonly ICuentaRepository _cuentaRepository;
    private readonly IClienteReferenciaRepository _clienteReferenciaRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="CreateCuentaCommandHandler"/>.
    /// </summary>
    /// <param name="cuentaRepository">
    /// Repositorio utilizado para consultar y registrar cuentas.
    /// </param>
    /// <param name="clienteReferenciaRepository">
    /// Repositorio utilizado para consultar la referencia local del cliente.
    /// </param>
    /// <param name="unitOfWork">
    /// Unidad de trabajo utilizada para persistir los cambios.
    /// </param>
    public CreateCuentaCommandHandler(
        ICuentaRepository cuentaRepository,
        IClienteReferenciaRepository clienteReferenciaRepository,
        IUnitOfWork unitOfWork)
    {
        _cuentaRepository = cuentaRepository;
        _clienteReferenciaRepository = clienteReferenciaRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Crea una nueva cuenta asociada a un cliente existente.
    /// </summary>
    /// <param name="request">Datos necesarios para crear la cuenta.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Información de la cuenta creada.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Se produce cuando el cliente no existe.
    /// </exception>
    /// <exception cref="DomainException">
    /// Se produce cuando el número de cuenta ya está registrado.
    /// </exception>
    public async Task<CuentaResponse> Handle(
        CreateCuentaCommand request,
        CancellationToken cancellationToken)
    {
        ClienteReferencia? cliente =
            await _clienteReferenciaRepository.GetByIdAsync(
                request.ClienteId,
                cancellationToken);

        if (cliente is null)
        {
            throw new KeyNotFoundException(
                "El cliente no existe.");
        }

        Cuenta? cuentaExistente =
            await _cuentaRepository.GetByNumeroCuentaAsync(
                request.NumeroCuenta,
                cancellationToken);

        if (cuentaExistente is not null)
        {
            throw new DomainException(
                $"El número de cuenta {request.NumeroCuenta} ya se encuentra registrado.");
        }

        SaldoInicial saldoInicial =
            SaldoInicial.Create(request.SaldoInicial);

        Cuenta cuenta = Cuenta.Create(
            request.NumeroCuenta,
            request.TipoCuenta,
            saldoInicial,
            true,
            request.ClienteId);

        _cuentaRepository.Add(cuenta);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new CuentaResponse(
            cuenta.Id.Value,
            cuenta.NumeroCuenta,
            cuenta.TipoCuenta,
            cuenta.SaldoInicial.Valor,
            cuenta.Estado,
            cliente.Nombre);
    }
}