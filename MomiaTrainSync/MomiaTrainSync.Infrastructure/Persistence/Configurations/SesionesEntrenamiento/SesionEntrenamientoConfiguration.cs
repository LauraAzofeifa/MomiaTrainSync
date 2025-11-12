using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MomiaTrainSync.Domain.Entities.SesionesEntrenamiento;
using System;

namespace MomiaTrainSync.Infrastructure.Persistence.Configurations.SesionesEntrenamiento
{
    public class SesionEntrenamientoConfiguration : IEntityTypeConfiguration<SesionEntrenamientoEnt>
    {
        public void Configure(EntityTypeBuilder<SesionEntrenamientoEnt> builder)
        {
            builder.ToTable("SesionEntrenamiento");

            // Primary Key
            builder.HasKey(s => s.IdSesion);
            builder.Property(s => s.IdSesion)
                .ValueGeneratedOnAdd();

            // Propiedades
            builder.Property(s => s.FechaEjecucion)
                .IsRequired();

            builder.Property(s => s.DuracionReal)
                .IsRequired();

            builder.Property(s => s.NivelEsfuerzoPercibido)
                .IsRequired();

            builder.Property(s => s.CargaTotal)
                .HasColumnType("decimal(6,2)")
                .IsRequired();

            builder.Property(s => s.Comentarios)
                .HasColumnType("text");

            // Relaciones
            builder.HasOne(s => s.Entrenamiento)
                .WithMany(a => a.Sesiones)
                .HasForeignKey(s => s.IdEntrenamiento)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.DetallesZona)
                .WithOne(d => d.Sesion)
                .HasForeignKey(d => d.IdSesion)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
