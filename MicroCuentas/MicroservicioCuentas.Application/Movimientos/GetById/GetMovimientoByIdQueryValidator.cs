using FluentValidation;

namespace MicroservicioCuentas.Application.Movimientos.GetById;

/// <summary>
/// Valida los parámetros utilizados para consultar un movimiento mediante su identificador.
/// </summary>
public class GetMovimientoByIdQueryValidator
    : AbstractValidator<GetMovimientoByIdQuery>
{
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="GetMovimientoByIdQueryValidator"/>
    /// y configura las reglas de validación de la consulta.
    /// </summary>
    public GetMovimientoByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("El Id debe ser mayor que 0.");
    }
}