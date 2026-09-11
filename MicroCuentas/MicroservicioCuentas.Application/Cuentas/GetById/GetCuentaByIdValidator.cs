using FluentValidation;

namespace MicroservicioCuentas.Application.Cuentas.GetById;

/// <summary>
/// Valida los datos utilizados para consultar una cuenta mediante su identificador.
/// </summary>
public class GetCuentaByIdQueryValidator
    : AbstractValidator<GetCuentaByIdQuery>
{
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="GetCuentaByIdQueryValidator"/>
    /// y configura las reglas de validación de la consulta.
    /// </summary>
    public GetCuentaByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("El Id debe ser mayor que 0.");
    }
}