namespace MicroservicioClientes.Infrastructure.Messaging;

/// <summary>
/// Contiene la configuración necesaria para la conexión y comunicación con RabbitMQ.
/// </summary>
public class RabbitMQOptions
{
    /// <summary>
    /// Obtiene o establece el host de RabbitMQ.
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el puerto de RabbitMQ.
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Obtiene o establece el nombre de usuario de RabbitMQ.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la contraseña de RabbitMQ.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el nombre del exchange.
    /// </summary>
    public string Exchange { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el nombre de la cola.
    /// </summary>
    public string Queue { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la clave de enrutamiento utilizada para publicar mensajes.
    /// </summary>
    public string RoutingKey { get; set; } = string.Empty;
}