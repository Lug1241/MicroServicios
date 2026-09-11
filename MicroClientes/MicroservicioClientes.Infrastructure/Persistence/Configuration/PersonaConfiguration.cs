using MicroservicioClientes.Domain.Clientes;
using MicroservicioClientes.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroservicioClientes.Infrastructure.Persistence.Configuration;

/// <summary>
/// Configura la persistencia de la entidad <see cref="Persona"/>.
/// </summary>
public class PersonaConfiguration : IEntityTypeConfiguration<Persona>
{
    /// <summary>
    /// Configura el mapeo de <see cref="Persona"/> hacia la base de datos.
    /// </summary>
    /// <param name="builder">Constructor utilizado para configurar la entidad.</param>
    public void Configure(EntityTypeBuilder<Persona> builder)
    {
        builder.ToTable("Persona");

        builder.HasKey(persona => persona.Id);

        builder.Property(persona => persona.Id)
            .HasConversion(
                clienteId => clienteId.Value,
                value => new ClienteId(value))
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(persona => persona.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(persona => persona.Genero)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(persona => persona.Edad)
            .HasConversion(
                edad => edad.Valor,
                valor => Edad.Create(valor))
            .IsRequired();

        builder.Property(persona => persona.Direccion)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(persona => persona.Identificacion)
            .HasConversion(
                identificacion => identificacion.Valor,
                valor => Identificacion.Create(valor))
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(persona => persona.Identificacion)
            .IsUnique();

        builder.Property(persona => persona.Telefono)
            .HasConversion(
                telefono => telefono.Valor,
                valor => Telefono.Create(valor))
            .IsRequired()
            .HasMaxLength(20);
    }
}