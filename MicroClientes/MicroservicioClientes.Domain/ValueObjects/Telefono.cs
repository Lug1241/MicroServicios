namespace MicroservicioClientes.Domain.ValueObjects;

/// <summary>
/// Representa un número de teléfono válido dentro del dominio.
/// </summary>
public record Telefono
{
    private const int DefaultLength = 10;

    /// <summary>
    /// Obtiene el valor del número de teléfono.
    /// </summary>
    public string Valor { get; init; }

    private Telefono(string valor)
    {
        Valor = valor;
    }

    /// <summary>
    /// Crea un número de teléfono validando su formato.
    /// </summary>
    /// <param name="valor">Número de teléfono que se desea validar.</param>
    /// <returns>Una instancia válida de <see cref="Telefono"/>.</returns>
    /// <exception cref="DomainException">
    /// Se produce cuando el número de teléfono no cumple las reglas establecidas.
    /// </exception>
    public static Telefono Create(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new DomainException(
                "El número de teléfono no puede estar vacío.");
        }

        if (valor.Length != DefaultLength)
        {
            throw new DomainException(
                "El número de teléfono debe tener 10 dígitos.");
        }

        if (!valor.StartsWith("09"))
        {
            throw new DomainException(
                "El número de teléfono debe comenzar con 09.");
        }

        if (!valor.All(char.IsDigit))
        {
            throw new DomainException(
                "El número de teléfono solo puede contener dígitos.");
        }

        return new Telefono(valor);
    }
}