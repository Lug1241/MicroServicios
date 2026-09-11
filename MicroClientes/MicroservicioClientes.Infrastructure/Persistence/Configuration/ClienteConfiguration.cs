using MicroservicioClientes.Domain.Clientes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroservicioClientes.Infrastructure.Persistence.Configuration;

/// <summary>
/// Configura la persistencia de la entidad <see cref="Cliente"/>.
/// </summary>
public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    /// <summary>
    /// Configura el mapeo de <see cref="Cliente"/> hacia la base de datos.
    /// </summary>
    /// <param name="builder">Constructor utilizado para configurar la entidad.</param>
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Cliente");

        builder.Property(c => c.Contraseña)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Estado)
            .IsRequired();
    }
}