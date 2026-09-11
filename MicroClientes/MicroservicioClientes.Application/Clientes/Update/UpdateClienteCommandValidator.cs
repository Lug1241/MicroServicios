using FluentValidation;

namespace MicroservicioClientes.Application.Clientes.Update;

/// <summary>
/// Valida los datos necesarios para actualizar un cliente.
/// </summary>
public sealed class UpdateClienteCommandValidator
    : AbstractValidator<UpdateClienteCommand>
{
    /// <summary>
    /// Inicializa las reglas de validación para la actualización de clientes.
    /// </summary>
    public UpdateClienteCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

        RuleFor(command => command.Nombre)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.Direccion)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(command => command.Identificacion)
            .NotEmpty()
            .Length(10)
            .Matches(@"^\d+$")
            .WithMessage(
                "La identificación debe contener únicamente dígitos.");

        RuleFor(command => command.Telefono)
            .NotEmpty()
            .Length(10)
            .Matches(@"^\d+$")
            .WithMessage(
                "El teléfono debe contener únicamente dígitos.");

        RuleFor(command => command.Contraseña)
            .NotEmpty();
    }
}