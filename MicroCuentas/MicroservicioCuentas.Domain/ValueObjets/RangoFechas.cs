using System.Globalization;

namespace MicroservicioCuentas.Domain.ValueObjects;

/// <summary>
/// Representa un rango de fechas utilizado para realizar consultas.
/// </summary>
public record RangoFechas
{
    /// <summary>
    /// Obtiene la fecha inicial del rango.
    /// </summary>
    public DateTime FechaInicio { get; }

    /// <summary>
    /// Obtiene la fecha final del rango.
    /// </summary>
    public DateTime FechaFin { get; }

    private RangoFechas(
        DateTime fechaInicio,
        DateTime fechaFin)
    {
        FechaInicio = fechaInicio;
        FechaFin = fechaFin;
    }

    /// <summary>
    /// Crea un rango de fechas a partir de una cadena con formato
    /// <c>dd/MM/yyyy-dd/MM/yyyy</c>.
    /// </summary>
    /// <param name="valor">Cadena que contiene las fechas de inicio y fin.</param>
    /// <returns>Una instancia válida de <see cref="RangoFechas"/>.</returns>
    /// <exception cref="DomainException">
    /// Se produce cuando el valor está vacío, tiene un formato incorrecto,
    /// contiene fechas inválidas o la fecha de inicio es posterior a la fecha de fin.
    /// </exception>
    public static RangoFechas Create(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new DomainException(
                "El rango de fechas es obligatorio.");
        }

        string[] fechas = valor.Split('-');

        if (fechas.Length != 2)
        {
            throw new DomainException(
                "El formato debe ser dd/MM/yyyy-dd/MM/yyyy.");
        }

        bool fechaInicioValida = DateTime.TryParseExact(
            fechas[0],
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out DateTime fechaInicio);

        if (!fechaInicioValida)
        {
            throw new DomainException(
                "La fecha de inicio no tiene un formato válido.");
        }

        bool fechaFinValida = DateTime.TryParseExact(
            fechas[1],
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out DateTime fechaFin);

        if (!fechaFinValida)
        {
            throw new DomainException(
                "La fecha de fin no tiene un formato válido.");
        }

        if (fechaInicio > fechaFin)
        {
            throw new DomainException(
                "La fecha de inicio no puede ser mayor que la fecha de fin.");
        }

        return new RangoFechas(
            fechaInicio,
            fechaFin);
    }
}