using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MomiaTrainSync.Domain.Entities.SesionesEntrenamiento;
using System;

namespace MomiaTrainSync.Infrastructure.Persistence.Configurations.SesionesEntrenamiento
{
    public class DetalleZonaSesionConfiguration : IEntityTypeConfiguration<DetalleZonaSesionEnt>
    {
        public void Configure(EntityTypeBuilder<DetalleZonaSesionEnt> builder)
        {
            builder.ToTable("DetalleZonaSesion");

            // Primary Key
            builder.HasKey(d => d.IdDetalleZonaSesion);
            builder.Property(d => d.IdDetalleZonaSesion)
                .ValueGeneratedOnAdd();

            // Propiedades
            builder.Property(d => d.MinutosCompletados)
                .IsRequired();

            // Relaciones
            builder.HasOne(d => d.Sesion)
                .WithMany(s => s.DetallesZona)
                .HasForeignKey(d => d.IdSesion)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Zona)
                .WithMany(z => z.DetalleZonaSesiones)
                .HasForeignKey(d => d.IdZona)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
