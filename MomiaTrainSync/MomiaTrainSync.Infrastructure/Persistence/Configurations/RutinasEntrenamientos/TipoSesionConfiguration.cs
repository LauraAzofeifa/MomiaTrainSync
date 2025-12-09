using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MomiaTrainSync.Domain.Entities.RutinasEntrenamientos;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Infrastructure.Persistence.Configurations.RutinasEntrenamientos
{
    public class TipoSesionConfiguration : IEntityTypeConfiguration<TipoSesionEnt>
    {
        public void Configure(EntityTypeBuilder<TipoSesionEnt> builder)
        {
            builder.ToTable("TipoSesion");
            // Primary Key
            builder.HasKey(t => t.IdTipoSesion);
            builder.Property(t => t.IdTipoSesion)
                .ValueGeneratedOnAdd();

            // Propiedades
            builder.Property(t => t.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Estado)
                .IsRequired();

            builder.HasMany(t => t.Entrenamientos)
                .WithOne(e => e.TipoSesion)
                .HasForeignKey(e => e.IdTipoSesion)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
