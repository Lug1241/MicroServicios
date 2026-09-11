namespace MicroservicioCuentas.Domain.ClientesReferencia;

/// <summary>
/// Define las operaciones de persistencia relacionadas con las referencias de clientes.
/// </summary>
public interface IClienteReferenciaRepository
{
    /// <summary>
    /// Obtiene una referencia de cliente mediante su identificador.
    /// </summary>
    /// <param name="id">Identificador del cliente.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>
    /// La referencia del cliente encontrada o <see langword="null"/> si no existe.
    /// </returns>
    Task<ClienteReferencia?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Agrega una nueva referencia de cliente.
    /// </summary>
    /// <param name="cliente">Referencia del cliente que se desea agregar.</param>
    void Add(ClienteReferencia cliente);
}