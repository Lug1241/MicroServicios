using MicroservicioClientes.Application.Events;
using MicroservicioClientes.Domain.Clientes;
using MicroservicioClientes.Domain.Primitives;
using MicroservicioClientes.Infrastructure.Messaging;
using MicroservicioClientes.Infrastructure.Persistence.Repositories;
using MicroservicioClientes.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;



namespace MicroservicioClientes.Infrastructure;
/// <summary>
/// Configura las dependencias de la capa de infraestructura.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra las dependencias de persistencia y mensajería.
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

        services.AddScoped<
            IEventPublisher,
            RabbitMQEventPublisher>();

        return services;
    }

    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ClientesDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("ClientesDbConnection"),
                sqlServerOptions =>
                    sqlServerOptions.MigrationsAssembly(
                        typeof(ClientesDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork>(serviceProvider =>
            serviceProvider.GetRequiredService<ClientesDbContext>());

        services.AddScoped<
            IClienteRepository,
            ClienteRepository>();

        return services;
    }
}