using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroservicioCuentas.Domain.ClientesReferencia;

namespace MicroservicioCuentas.Infrastructure.Persistence.Configuration;

/// <summary>
/// Configura el mapeo de la entidad <see cref="ClienteReferencia"/>
/// hacia la base de datos.
/// </summary>
public class ClienteReferenciaConfiguration
    : IEntityTypeConfiguration<ClienteReferencia>
{
    /// <summary>
    /// Configura las propiedades de la entidad <see cref="ClienteReferencia"/>.
    /// </summary>
    /// <param name="builder">
    /// Constructor utilizado para configurar la entidad.
    /// </param>
    public void Configure(
        EntityTypeBuilder<ClienteReferencia> builder)
    {
        builder.ToTable("ClienteReferencia");

        builder.HasKey(cliente => cliente.ClienteId);

        builder.Property(cliente => cliente.ClienteId)
            .ValueGeneratedNever();

        builder.Property(cliente => cliente.Nombre)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(cliente => cliente.Estado)
            .IsRequired();
    }
}