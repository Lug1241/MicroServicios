using MicroservicioCuentas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MicroservicioClientes.Api.Extensions;

/// <summary>
/// Proporciona extensiones relacionadas con las migraciones de la base de datos.
/// </summary>
public static class MigrationExtensions
{
    /// <summary>
    /// Aplica las migraciones pendientes de la base de datos de clientes.
    /// </summary>
    /// <param name="app">Aplicación web que contiene el contexto de persistencia.</param>
    public static void ApplyMigrations(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        CuentasDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<CuentasDbContext>();

        dbContext.Database.Migrate();
    }
}