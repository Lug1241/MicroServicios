using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MicroservicioCuentas.Application.Behaviors;

namespace MicroservicioCuentas.Application;

/// <summary>
/// Configura las dependencias de la capa de aplicación.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra los servicios utilizados por la capa de aplicación.
    /// </summary>
    /// <param name="services">Colección de servicios de la aplicación.</param>
    /// <returns>La colección de servicios configurada.</returns>
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<
                ApplicationAssemblyReference>();
        });

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssemblyContaining<
            ApplicationAssemblyReference>();

        return services;
    }
}