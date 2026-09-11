namespace MicroservicioCuentas.Infrastructure.Messaging;

/// <summary>
/// Contiene la configuración necesaria para establecer la conexión
/// y consumir mensajes desde RabbitMQ.
/// </summary>
public class RabbitMQOptions
{
    /// <summary>
    /// Obtiene o establece el nombre del host de RabbitMQ.
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el puerto utilizado por RabbitMQ.
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Obtiene o establece el nombre de usuario para conectarse a RabbitMQ.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la contraseña para conectarse a RabbitMQ.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el nombre del exchange utilizado para los eventos.
    /// </summary>
    public string Exchange { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el nombre de la cola que consumirá los mensajes.
    /// </summary>
    public string Queue { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la clave de enrutamiento utilizada por RabbitMQ.
    /// </summary>
    public string RoutingKey { get; set; } = string.Empty;
}