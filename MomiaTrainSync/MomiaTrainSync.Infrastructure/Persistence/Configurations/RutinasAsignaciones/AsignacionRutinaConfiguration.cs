using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MomiaTrainSync.Domain.Entities.RutinasAsignaciones;
using System;

namespace MomiaTrainSync.Infrastructure.Persistence.Configurations.RutinasAsignaciones
{
    public class AsignacionRutinaConfiguration : IEntityTypeConfiguration<AsignacionRutinaEnt>
    {
        public void Configure(EntityTypeBuilder<AsignacionRutinaEnt> builder)
        {
            builder.ToTable("AsignacionRutina");

            // Primary Key
            builder.HasKey(a => a.IdAsignacion);
            builder.Property(a => a.IdAsignacion)
                .ValueGeneratedOnAdd();

            // Propiedades
            builder.Property(a => a.FechaProgramada)
                .IsRequired();

            builder.Property(a => a.Estado)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(a => a.NotaEntrenador)
                .HasColumnType("text");

            // Relaciones
            builder.HasOne(a => a.Rutina)
                .WithMany(r => r.Asignaciones)
                .HasForeignKey(a => a.IdRutina)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Entrenamiento)
                .WithMany(e => e.Asignaciones)
                .HasForeignKey(a => a.IdEntrenamiento)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Relacion)
                .WithMany(ea => ea.Asignaciones)
                .HasForeignKey(a => a.IdRelacion)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Sesiones)
                .WithOne(s => s.Asignacion)
                .HasForeignKey(s => s.IdAsignacion)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
