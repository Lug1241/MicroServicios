using FluentValidation;

namespace MicroservicioClientes.Application.Clientes.Delete;

/// <summary>
/// Valida los datos necesarios para eliminar un cliente.
/// </summary>
public sealed class DeleteClienteCommandValidator
    : AbstractValidator<DeleteClienteCommand>
{
    /// <summary>
    /// Inicializa las reglas de validación para la eliminación de clientes.
    /// </summary>
    public DeleteClienteCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
    }
}