using FluentValidation;
using MediatR;

namespace Application.Common.Behaviors;

/// <summary>
/// Ejecuta la validación de una solicitud antes de enviarla a su handler.
/// </summary>
/// <typeparam name="TRequest">Tipo de la solicitud que se desea validar.</typeparam>
/// <typeparam name="TResponse">Tipo de respuesta de la solicitud.</typeparam>
public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest>? _validator;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="ValidationBehavior{TRequest, TResponse}"/>.
    /// </summary>
    /// <param name="validator">
    /// Validador correspondiente a la solicitud, si existe.
    /// </param>
    public ValidationBehavior(
        IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    /// <summary>
    /// Valida la solicitud antes de ejecutar el siguiente comportamiento o handler.
    /// </summary>
    /// <param name="request">Solicitud que se desea validar.</param>
    /// <param name="next">Siguiente comportamiento del pipeline.</param>
    /// <param name="cancellationToken">Token para cancelar la operación.</param>
    /// <returns>La respuesta generada por el siguiente comportamiento o handler.</returns>
    /// <exception cref="ValidationException">
    /// Se produce cuando la solicitud no cumple las reglas de validación.
    /// </exception>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator is null)
        {
            return await next();
        }

        FluentValidation.Results.ValidationResult validationResult =
            await _validator.ValidateAsync(
                request,
                cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(
                validationResult.Errors);
        }

        return await next();
    }
}