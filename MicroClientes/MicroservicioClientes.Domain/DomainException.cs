namespace MicroservicioClientes.Domain;

/// <summary>
/// Representa una excepción producida por una regla del dominio.
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="DomainException"/>.
    /// </summary>
    /// <param name="message">Mensaje que describe la regla de dominio incumplida.</param>
    public DomainException(string message) : base(message)
    {
    }
}