using FluentValidation;

namespace MicroservicioCuentas.Application.Cuentas.Update;

/// <summary>
/// Valida los datos utilizados para actualizar una cuenta.
/// </summary>
public class UpdateCuentaCommandValidator
    : AbstractValidator<UpdateCuentaCommand>
{
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="UpdateCuentaCommandValidator"/>
    /// y configura las reglas de validación del comando.
    /// </summary>
    public UpdateCuentaCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("El Id debe ser mayor que 0.");
    }
}