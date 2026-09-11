using FluentValidation;

namespace MicroservicioCuentas.Application.Movimientos.Create;

/// <summary>
/// Valida los datos utilizados para crear un nuevo movimiento.
/// </summary>
public class CreateMovimientoCommandValidator
    : AbstractValidator<CreateMovimientoCommand>
{
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="CreateMovimientoCommandValidator"/>
    /// y configura las reglas de validación del comando.
    /// </summary>
    public CreateMovimientoCommandValidator()
    {
        RuleFor(x => x.CuentaId)
            .GreaterThan(0)
            .WithMessage("El CuentaId debe ser mayor que 0.");

        RuleFor(x => x.TipoMovimiento)
            .IsInEnum()
            .WithMessage("El tipo de movimiento no es válido.");

        RuleFor(x => x.Valor)
            .NotEqual(0)
            .WithMessage("El valor del movimiento no puede ser cero.");
    }
}