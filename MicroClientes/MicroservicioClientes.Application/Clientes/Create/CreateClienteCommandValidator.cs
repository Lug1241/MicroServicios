using FluentValidation;

namespace MicroservicioClientes.Application.Clientes.Create;

/// <summary>
/// Valida los datos necesarios para crear un cliente.
/// </summary>
public sealed class CreateClienteCommandValidator
    : AbstractValidator<CreateClienteCommand>
{
    /// <summary>
    /// Inicializa las reglas de validación para la creación de clientes.
    /// </summary>
    public CreateClienteCommandValidator()
    {
        RuleFor(cliente => cliente.Nombre)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(cliente => cliente.Direccion)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(cliente => cliente.Identificacion)
            .NotEmpty()
            .Length(10)
            .Matches(@"^\d+$")
            .WithMessage(
                "La identificación debe contener únicamente dígitos.");

        RuleFor(cliente => cliente.Telefono)
            .NotEmpty()
            .Length(10)
            .Matches(@"^\d+$")
            .WithMessage(
                "El teléfono debe contener únicamente dígitos.");

        RuleFor(cliente => cliente.Contraseña)
            .NotEmpty()
            .MinimumLength(6);
    }
}