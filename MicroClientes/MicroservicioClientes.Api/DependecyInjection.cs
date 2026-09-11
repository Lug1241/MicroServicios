using MicroservicioClientes.Api.Middlewares;

namespace MicroservicioClientes.Api;

/// <summary>
/// Configura las dependencias de la capa de presentación.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra los servicios utilizados por la capa de presentación.
    /// </summary>
    /// <param name="services">Colección de servicios de la aplicación.</param>
    /// <returns>La colección de servicios configurada.</returns>
    public static IServiceCollection AddPresentation(
        this IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        services.AddTransient<GlobalExceptionHandlingMiddleware>();

        return services;
    }
}