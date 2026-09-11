using FluentValidation;

namespace MicroservicioCuentas.Application.Cuentas.Create;

/// <summary>
/// Valida los datos utilizados para crear una nueva cuenta.
/// </summary>
public class CreateCuentaCommandValidator
    : AbstractValidator<CreateCuentaCommand>
{
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="CreateCuentaCommandValidator"/>
    /// y configura las reglas de validación del comando.
    /// </summary>
    public CreateCuentaCommandValidator()
    {
        RuleFor(x => x.ClienteId)
            .GreaterThan(0)
            .WithMessage("El ClienteId debe ser mayor que 0.");

        RuleFor(x => x.SaldoInicial)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El saldo inicial no puede ser negativo.");

        RuleFor(x => x.TipoCuenta)
            .IsInEnum()
            .WithMessage("El tipo de cuenta no es válido.");
    }
}