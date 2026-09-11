using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroservicioCuentas.Domain.ClientesReferencia;
using MicroservicioCuentas.Domain.Cuentas;
using MicroservicioCuentas.Domain.Movimiento;
using MicroservicioCuentas.Domain.Primitives;
using MicroservicioCuentas.Infrastructure.Messaging;
using MicroservicioCuentas.Infrastructure.Persistence;
using MicroservicioCuentas.Infrastructure.Persistence.Repositories;

namespace MicroservicioCuentas.Infrastructure;

/// <summary>
/// Configura las dependencias de la capa de infraestructura.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra los servicios de persistencia y mensajería utilizados
    /// por el microservicio de cuentas.
    /// </summary>
    /// <param name="services">Colección de servicios de la aplicación.</param>
    /// <param name="configuration">Configuración de la aplicación.</param>
    /// <returns>La colección de servicios configurada.</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPersistence(configuration);

        services.Configure<RabbitMQOptions>(
            configuration.GetSection("RabbitMQ"));

        services.AddHostedService<ClienteActualizadoConsumer>();

        return services;
    }

    /// <summary>
    /// Registra los servicios relacionados con la persistencia de datos.
    /// </summary>
    /// <param name="services">Colección de servicios de la aplicación.</param>
    /// <param name="configuration">Configuración de la aplicación.</param>
    /// <returns>La colección de servicios configurada.</returns>
    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CuentasDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString(
                    "CuentasDbConnection"),
                sqlServerOptions =>
                    sqlServerOptions.MigrationsAssembly(
                        typeof(CuentasDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork>(serviceProvider =>
            serviceProvider.GetRequiredService<CuentasDbContext>());

        services.AddScoped<
            ICuentaRepository,
            CuentaRepository>();

        services.AddScoped<
            IMovimientoRepository,
            MovimientoRepository>();

        services.AddScoped<
            IClienteReferenciaRepository,
            ClienteReferenciaRepository>();

        return services;
    }
}