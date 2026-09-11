namespace MicroservicioCuentas.Domain.ValueObjets;

/// <summary>
/// Representa el saldo inicial de una cuenta.
/// </summary>
public record SaldoInicial
{
    /// <summary>
    /// Obtiene el valor del saldo inicial.
    /// </summary>
    public decimal Valor { get; init; }

    private SaldoInicial(decimal valor)
    {
        Valor = valor;
    }

    /// <summary>
    /// Crea un saldo inicial validando que no sea negativo.
    /// </summary>
    /// <param name="valor">Valor del saldo inicial.</param>
    /// <returns>Una instancia válida de <see cref="SaldoInicial"/>.</returns>
    /// <exception cref="DomainException">
    /// Se produce cuando el saldo inicial es menor que cero.
    /// </exception>
    public static SaldoInicial Create(decimal valor)
    {
        if (valor < 0)
        {
            throw new DomainException(
                "El saldo inicial debe ser mayor o igual a cero.");
        }

        return new SaldoInicial(valor);
    }
}