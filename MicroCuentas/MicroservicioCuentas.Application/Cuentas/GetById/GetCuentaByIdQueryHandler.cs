using MediatR;
using MicroservicioCuentas.Domain.ClientesReferencia;
using MicroservicioCuentas.Domain.Cuentas;

namespace MicroservicioCuentas.Application.Cuentas.GetById;

/// <summary>
/// Maneja la consulta de una cuenta mediante su identificador.
/// </summary>
public class GetCuentaByIdQueryHandler
    : IRequestHandler<GetCuentaByIdQuery, CuentaResponse>
{
    private readonly ICuentaRepository _cuentaRepository;
    private readonly IClienteReferenciaRepository _clienteReferenciaRepository;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="GetCuentaByIdQueryHandler"/>.
    /// </summary>
    /// <param name="cuentaRepository">
    /// Repositorio utilizado para consultar las cuentas.
    /// </param>
    /// <param name="clienteReferenciaRepository">
    /// Repositorio utilizado para consultar la referencia local del cliente.
    /// </param>
    public GetCuentaByIdQueryHandler(
        ICuentaRepository cuentaRepository,
        IClienteReferenciaRepository clienteReferenciaRepository)
    {
        _cuentaRepository = cuentaRepository;
        _clienteReferenciaRepository = clienteReferenciaRepository;
    }

    /// <summary>
    /// Obtiene una cuenta y el nombre del cliente asociado.
    /// </summary>
    /// <param name="request">Parámetros de la consulta.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Información de la cuenta consultada.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Se produce cuando la cuenta o el cliente asociado no existen.
    /// </exception>
    public async Task<CuentaResponse> Handle(
        GetCuentaByIdQuery request,
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

        ClienteReferencia? cliente =
            await _clienteReferenciaRepository.GetByIdAsync(
                cuenta.ClienteId,
                cancellationToken);

        if (cliente is null)
        {
            throw new KeyNotFoundException(
                "El cliente no existe.");
        }

        return new CuentaResponse(
            cuenta.Id.Value,
            cuenta.NumeroCuenta,
            cuenta.TipoCuenta,
            cuenta.SaldoInicial.Valor,
            cuenta.Estado,
            cliente.Nombre);
    }
}