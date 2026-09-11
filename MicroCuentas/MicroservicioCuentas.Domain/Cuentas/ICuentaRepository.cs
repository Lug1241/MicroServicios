namespace MicroservicioCuentas.Domain.Cuentas;

/// <summary>
/// Define las operaciones de persistencia relacionadas con las cuentas.
/// </summary>
public interface ICuentaRepository
{
    /// <summary>
    /// Obtiene una cuenta mediante su identificador.
    /// </summary>
    /// <param name="id">Identificador de la cuenta.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>La cuenta encontrada o <see langword="null"/> si no existe.</returns>
    Task<Cuenta?> GetByIdAsync(
        CuentaId id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene una cuenta mediante su número de cuenta.
    /// </summary>
    /// <param name="numeroCuenta">Número de cuenta que se desea consultar.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>La cuenta encontrada o <see langword="null"/> si no existe.</returns>
    Task<Cuenta?> GetByNumeroCuentaAsync(
        int numeroCuenta,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene las cuentas asociadas a un cliente.
    /// </summary>
    /// <param name="clienteId">Identificador del cliente.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Lista de cuentas asociadas al cliente.</returns>
    Task<List<Cuenta>> GetByClienteIdAsync(
        int clienteId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Agrega una nueva cuenta.
    /// </summary>
    /// <param name="cuenta">Cuenta que se desea agregar.</param>
    void Add(Cuenta cuenta);

    /// <summary>
    /// Actualiza una cuenta existente.
    /// </summary>
    /// <param name="cuenta">Cuenta que se desea actualizar.</param>
    void Update(Cuenta cuenta);
}