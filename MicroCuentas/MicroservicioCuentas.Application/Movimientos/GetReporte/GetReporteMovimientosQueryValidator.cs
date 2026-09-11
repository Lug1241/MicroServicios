using FluentValidation;

namespace MicroservicioCuentas.Application.Movimientos.GetReporte;

/// <summary>
/// Valida los parámetros utilizados para consultar el reporte de movimientos.
/// </summary>
public class GetReporteMovimientosQueryValidator
    : AbstractValidator<GetReporteMovimientosQuery>
{
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="GetReporteMovimientosQueryValidator"/>
    /// y configura las reglas de validación de la consulta.
    /// </summary>
    public GetReporteMovimientosQueryValidator()
    {
        RuleFor(x => x.ClienteId)
            .GreaterThan(0)
            .WithMessage("El ClienteId debe ser mayor que 0.");

        RuleFor(x => x.FechaInicio)
            .NotEmpty()
            .WithMessage("La fecha de inicio es obligatoria.");

        RuleFor(x => x.FechaFin)
            .NotEmpty()
            .WithMessage("La fecha de fin es obligatoria.");

        RuleFor(x => x)
            .Must(x => x.FechaInicio <= x.FechaFin)
            .WithMessage(
                "La fecha de inicio no puede ser mayor que la fecha de fin.");
    }
}