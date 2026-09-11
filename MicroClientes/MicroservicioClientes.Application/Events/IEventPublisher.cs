namespace MicroservicioClientes.Application.Events;

/// <summary>
/// Define la operación para publicar eventos de integración.
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publica un evento de forma asíncrona.
    /// </summary>
    /// <typeparam name="T">Tipo del evento que se desea publicar.</typeparam>
    /// <param name="evento">Evento que se desea publicar.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    Task PublishAsync<T>(
        T evento,
        CancellationToken cancellationToken = default);
}