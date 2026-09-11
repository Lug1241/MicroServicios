namespace MicroservicioClientes.Domain.ValueObjects;

/// <summary>
/// Representa la edad de un cliente y valida que se encuentre dentro del rango permitido.
/// </summary>
public record Edad
{
    /// <summary>
    /// Obtiene el valor de la edad.
    /// </summary>
    public int Valor { get; init; }

    private Edad(int valor)
    {
        Valor = valor;
    }

    /// <summary>
    /// Crea una instancia de <see cref="Edad"/> validando el rango permitido.
    /// </summary>
    /// <param name="valor">Valor de la edad en años.</param>
    /// <returns>Una instancia válida de <see cref="Edad"/>.</returns>
    /// <exception cref="DomainException">
    /// Se produce cuando la edad es menor que 0 o mayor que 100.
    /// </exception>
    public static Edad Create(int valor)
    {
        if (valor < 0 || valor > 100)
        {
            throw new DomainException(
                "La edad debe estar entre 0 y 100 años.");
        }

        return new Edad(valor);
    }
}