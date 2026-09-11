namespace MicroservicioClientes.Domain.ValueObjects;

/// <summary>
/// Representa una identificación válida dentro del dominio.
/// </summary>
public record Identificacion
{
    /// <summary>
    /// Obtiene el valor de la identificación.
    /// </summary>
    public string Valor { get; init; }

    private Identificacion(string valor)
    {
        Valor = valor;
    }

    /// <summary>
    /// Crea una identificación validando su formato y su número de cédula ecuatoriana.
    /// </summary>
    /// <param name="valor">Número de identificación.</param>
    /// <returns>Una identificación válida.</returns>
    /// <exception cref="DomainException">
    /// Se produce cuando la identificación no cumple alguna de las reglas establecidas.
    /// </exception>
    public static Identificacion Create(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new DomainException(
                "La identificación no puede estar vacía.");
        }

        if (valor.Length != 10)
        {
            throw new DomainException(
                "La identificación debe tener 10 dígitos.");
        }

        if (!valor.All(char.IsDigit))
        {
            throw new DomainException(
                "La identificación solo puede contener dígitos.");
        }

        if (!EsCedulaValida(valor))
        {
            throw new DomainException(
                "La identificación no es una cédula ecuatoriana válida.");
        }

        return new Identificacion(valor);
    }

    /// <summary>
    /// Verifica si una identificación corresponde a una cédula ecuatoriana válida.
    /// </summary>
    /// <param name="cedula">Número de cédula que se desea validar.</param>
    /// <returns><c>true</c> si la cédula es válida; de lo contrario, <c>false</c>.</returns>
    private static bool EsCedulaValida(string cedula)
    {
        int provincia = int.Parse(cedula[..2]);

        if (provincia < 1 || provincia > 24)
            return false;

        int[] coeficientes = { 2, 1, 2, 1, 2, 1, 2, 1, 2 };

        int suma = 0;

        for (int i = 0; i < 9; i++)
        {
            int resultado =
                int.Parse(cedula[i].ToString()) * coeficientes[i];

            if (resultado >= 10)
                resultado -= 9;

            suma += resultado;
        }

        int decenaSuperior = ((suma / 10) + 1) * 10;
        int digitoVerificador = decenaSuperior - suma;

        if (digitoVerificador == 10)
            digitoVerificador = 0;

        return digitoVerificador ==
               int.Parse(cedula[9].ToString());
    }
}