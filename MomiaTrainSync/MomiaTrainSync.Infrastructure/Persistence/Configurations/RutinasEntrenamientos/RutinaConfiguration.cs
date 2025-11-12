using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MomiaTrainSync.Domain.Entities.RutinasAsignaciones;
using System;

namespace MomiaTrainSync.Infrastructure.Persistence.Configurations.RutinasAsignaciones
{
    public class RutinaConfiguration : IEntityTypeConfiguration<RutinaEnt>
    {
        public void Configure(EntityTypeBuilder<RutinaEnt> builder)
        {
            builder.ToTable("Rutina");

            // Primary Key
            builder.HasKey(r => r.IdRutina);
            builder.Property(r => r.IdRutina)
                .ValueGeneratedOnAdd();

            // Propiedades
            builder.Property(r => r.Nombre)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(r => r.Descripcion)
                .HasMaxLength(255);

            builder.Property(r => r.FechaCreacion)
                .IsRequired();

            builder.Property(r => r.Estado)
                .IsRequired();

            // Relaciones
            builder.HasMany(r => r.Entrenamientos)
                .WithOne(a => a.Rutina)
                .HasForeignKey(a => a.IdRutina)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
