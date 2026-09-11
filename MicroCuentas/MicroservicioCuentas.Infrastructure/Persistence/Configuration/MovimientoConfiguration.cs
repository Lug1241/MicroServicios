using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MicroservicioCuentas.Domain.Cuentas;
using MicroservicioCuentas.Domain.Movimiento;

namespace MicroservicioCuentas.Infrastructure.Persistence.Configuration;

/// <summary>
/// Configura el mapeo de la entidad <see cref="Movimiento"/> hacia la base de datos.
/// </summary>
public class MovimientoConfiguration : IEntityTypeConfiguration<Movimiento>
{
    /// <summary>
    /// Configura las propiedades y relaciones de la entidad <see cref="Movimiento"/>.
    /// </summary>
    /// <param name="builder">
    /// Constructor utilizado para configurar la entidad.
    /// </param>
    public void Configure(EntityTypeBuilder<Movimiento> builder)
    {
        builder.ToTable("Movimiento");

        builder.HasKey(movimiento => movimiento.Id);

        builder.Property(movimiento => movimiento.Id)
            .HasConversion(
                movimientoId => movimientoId.Value,
                value => new MovimientoId(value))
            .ValueGeneratedOnAdd();

        builder.Property(movimiento => movimiento.Fecha)
            .IsRequired();

        builder.Property(movimiento => movimiento.TipoMovimiento)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(movimiento => movimiento.Valor)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(movimiento => movimiento.Saldo)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(movimiento => movimiento.CuentaId)
            .HasConversion(
                cuentaId => cuentaId.Value,
                value => new CuentaId(value))
            .IsRequired();

        builder.HasOne<Cuenta>()
            .WithMany()
            .HasForeignKey(movimiento => movimiento.CuentaId)
            .HasPrincipalKey(cuenta => cuenta.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}