using MicroservicioClientes.Domain.ValueObjects;

namespace MicroservicioClientes.Domain.Clientes;

/// <summary>
/// Define las operaciones de persistencia relacionadas con los clientes.
/// </summary>
public interface IClienteRepository
{
    /// <summary>
    /// Obtiene un cliente mediante su identificador.
    /// </summary>
    /// <param name="id">Identificador del cliente.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>El cliente encontrado o <c>null</c> si no existe.</returns>
    Task<Cliente?> GetByIdAsync(
        ClienteId id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un cliente mediante su identificación.
    /// </summary>
    /// <param name="identificacion">Identificación del cliente.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>El cliente encontrado o <c>null</c> si no existe.</returns>
    Task<Cliente?> GetByIdentificacionAsync(
        Identificacion identificacion,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Agrega un nuevo cliente.
    /// </summary>
    /// <param name="cliente">Cliente que se desea agregar.</param>
    void Add(Cliente cliente);

    /// <summary>
    /// Actualiza un cliente existente.
    /// </summary>
    /// <param name="cliente">Cliente que se desea actualizar.</param>
    void Update(Cliente cliente);

    /// <summary>
    /// Elimina un cliente.
    /// </summary>
    /// <param name="cliente">Cliente que se desea eliminar.</param>
    void Delete(Cliente cliente);
}