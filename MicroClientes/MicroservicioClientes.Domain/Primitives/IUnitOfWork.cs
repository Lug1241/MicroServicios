namespace MicroservicioClientes.Domain.Primitives;

/// <summary>
/// Define la operación para persistir los cambios realizados en una unidad de trabajo.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Persiste de forma asíncrona los cambios pendientes.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>El número de registros afectados.</returns>
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}