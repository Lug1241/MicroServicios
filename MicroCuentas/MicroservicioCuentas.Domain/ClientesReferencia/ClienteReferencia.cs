namespace MicroservicioCuentas.Domain.ClientesReferencia;

/// <summary>
/// Representa la información local de un cliente utilizada como referencia
/// dentro del microservicio de cuentas.
/// </summary>
public class ClienteReferencia
{
    /// <summary>
    /// Obtiene el identificador del cliente en el microservicio de clientes.
    /// </summary>
    public int ClienteId { get; private set; }

    /// <summary>
    /// Obtiene el estado actual del cliente.
    /// </summary>
    public bool Estado { get; private set; }

    /// <summary>
    /// Obtiene el nombre del cliente.
    /// </summary>
    public string Nombre { get; private set; }

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="ClienteReferencia"/>.
    /// </summary>
    /// <param name="clienteId">Identificador del cliente.</param>
    /// <param name="estado">Estado actual del cliente.</param>
    /// <param name="nombre">Nombre del cliente.</param>
    private ClienteReferencia(
        int clienteId,
        bool estado,
        string nombre)
    {
        ClienteId = clienteId;
        Estado = estado;
        Nombre = nombre;
    }

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="ClienteReferencia"/>.
    /// </summary>
    private ClienteReferencia()
    {
    }

    /// <summary>
    /// Crea una nueva referencia de cliente.
    /// </summary>
    /// <param name="clienteId">Identificador del cliente.</param>
    /// <param name="estado">Estado actual del cliente.</param>
    /// <param name="nombre">Nombre del cliente.</param>
    /// <returns>Una nueva referencia de cliente.</returns>
    public static ClienteReferencia Create(
        int clienteId,
        bool estado,
        string nombre)
    {
        ClienteReferencia clienteReferencia = new ClienteReferencia(
            clienteId,
            estado,
            nombre);

        return clienteReferencia;
    }

    /// <summary>
    /// Actualiza el estado del cliente.
    /// </summary>
    /// <param name="estado">Nuevo estado del cliente.</param>
    public void ActualizarEstado(bool estado)
    {
        Estado = estado;
    }
}