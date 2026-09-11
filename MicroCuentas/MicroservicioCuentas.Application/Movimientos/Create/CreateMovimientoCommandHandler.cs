using MediatR;
using MicroservicioCuentas.Domain;
using MicroservicioCuentas.Domain.Cuentas;
using MicroservicioCuentas.Domain.Movimiento;
using MicroservicioCuentas.Domain.Primitives;

namespace MicroservicioCuentas.Application.Movimientos.Create;

/// <summary>
/// Maneja la creación de movimientos asociados a una cuenta.
/// </summary>
public class CreateMovimientoCommandHandler
    : IRequestHandler<CreateMovimientoCommand, MovimientoResponse>
{
    private readonly ICuentaRepository _cuentaRepository;
    private readonly IMovimientoRepository _movimientoRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="CreateMovimientoCommandHandler"/>.
    /// </summary>
    /// <param name="cuentaRepository">
    /// Repositorio utilizado para consultar las cuentas.
    /// </param>
    /// <param name="movimientoRepository">
    /// Repositorio utilizado para consultar y registrar movimientos.
    /// </param>
    /// <param name="unitOfWork">
    /// Unidad de trabajo utilizada para persistir los cambios.
    /// </param>
    public CreateMovimientoCommandHandler(
        ICuentaRepository cuentaRepository,
        IMovimientoRepository movimientoRepository,
        IUnitOfWork unitOfWork)
    {
        _cuentaRepository = cuentaRepository;
        _movimientoRepository = movimientoRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Crea un nuevo movimiento y calcula el saldo resultante de la cuenta.
    /// </summary>
    /// <param name="request">Datos necesarios para crear el movimiento.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Información de la cuenta y del movimiento creado.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Se produce cuando la cuenta no existe.
    /// </exception>
    /// <exception cref="DomainException">
    /// Se produce cuando el movimiento genera un saldo negativo.
    /// </exception>
    public async Task<MovimientoResponse> Handle(
        CreateMovimientoCommand request,
        CancellationToken cancellationToken)
    {
        CuentaId cuentaId =
            new CuentaId(request.CuentaId);

        Cuenta? cuenta =
            await _cuentaRepository.GetByIdAsync(
                cuentaId,
                cancellationToken);

        if (cuenta is null)
        {
            throw new KeyNotFoundException(
                "La cuenta no existe.");
        }

        Movimiento? ultimoMovimiento =
            await _movimientoRepository.GetUltimoMovimientoAsync(
                cuentaId,
                cancellationToken);

        decimal saldoAnterior = ultimoMovimiento is null
            ? cuenta.SaldoInicial.Valor
            : ultimoMovimiento.Saldo;

        decimal nuevoSaldo =
            saldoAnterior + request.Valor;

        if (nuevoSaldo < 0)
        {
            throw new DomainException(
                "Saldo no disponible.");
        }

        Movimiento movimiento = Movimiento.Create(
            DateTime.UtcNow,
            request.TipoMovimiento,
            request.Valor,
            nuevoSaldo,
            cuentaId);

        _movimientoRepository.Add(movimiento);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        string tipoMovimiento = movimiento.Valor >= 0
            ? "Deposito"
            : "Retiro";

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