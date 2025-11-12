using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MomiaTrainSync.Domain.Entities.EntrenamientosZonas;
using System;

namespace MomiaTrainSync.Infrastructure.Persistence.Configurations.EntrenamientoZonas
{
    public class EntrenamientoConfiguration : IEntityTypeConfiguration<EntrenamientoEnt>
    {
        public void Configure(EntityTypeBuilder<EntrenamientoEnt> builder)
        {
            builder.ToTable("Entrenamiento");

            // Primary Key
            builder.HasKey(e => e.IdEntrenamiento);
            builder.Property(e => e.IdEntrenamiento)
                .ValueGeneratedOnAdd();

            // Propiedades
            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(e => e.TipoSesion)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Objetivo)
                .HasMaxLength(255);

            builder.Property(e => e.DuracionEstimada)
                .IsRequired();

            builder.Property(e => e.NivelEsfuerzoEsperado)
                .IsRequired();

            builder.Property(e => e.Descripcion)
                .HasColumnType("text");

            builder.Property(e => e.FechaProgramada)
                .IsRequired();

            builder.Property(e => e.FechaCreacion)
                .IsRequired();

            builder.Property(e => e.Estado)
                .IsRequired();

            // Relaciones
            builder.HasOne(e => e.Rutina)
                .WithMany(r => r.Entrenamientos)
                .HasForeignKey(e => e.IdRutina)
                .OnDelete(DeleteBehavior.Cascade); // Si se elimina la rutina, se eliminan los entrenamientos

            builder.HasMany(e => e.DetallesZonaPlan)
                .WithOne(d => d.Entrenamiento)
                .HasForeignKey(d => d.IdEntrenamiento)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
