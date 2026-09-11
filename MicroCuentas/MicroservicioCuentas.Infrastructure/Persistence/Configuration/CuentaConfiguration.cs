using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroservicioCuentas.Domain.ClientesReferencia;
using MicroservicioCuentas.Domain.Cuentas;
using MicroservicioCuentas.Domain.ValueObjets;

namespace MicroservicioCuentas.Infrastructure.Persistence.Configuration;

/// <summary>
/// Configura el mapeo de la entidad <see cref="Cuenta"/> hacia la base de datos.
/// </summary>
public class CuentaConfiguration : IEntityTypeConfiguration<Cuenta>
{
    /// <summary>
    /// Configura las propiedades y relaciones de la entidad <see cref="Cuenta"/>.
    /// </summary>
    /// <param name="builder">
    /// Constructor utilizado para configurar la entidad.
    /// </param>
    public void Configure(EntityTypeBuilder<Cuenta> builder)
    {
        builder.ToTable("Cuenta");

        builder.HasKey(cuenta => cuenta.Id);

        builder.Property(cuenta => cuenta.Id)
            .HasConversion(
                cuentaId => cuentaId.Value,
                value => new CuentaId(value))
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(cuenta => cuenta.SaldoInicial)
            .HasConversion(
                saldo => saldo.Valor,
                valor => SaldoInicial.Create(valor))
            .IsRequired();

        builder.Property(cuenta => cuenta.NumeroCuenta)
            .IsRequired();

        builder.HasIndex(cuenta => cuenta.NumeroCuenta)
            .IsUnique();

        builder.Property(cuenta => cuenta.TipoCuenta)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(cuenta => cuenta.Estado)
            .IsRequired();

        builder.Property(cuenta => cuenta.ClienteId)
            .IsRequired();

        builder.HasOne<ClienteReferencia>()
            .WithMany()
            .HasForeignKey(cuenta => cuenta.ClienteId)
            .HasPrincipalKey(cliente => cliente.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}