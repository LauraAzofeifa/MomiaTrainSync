using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MomiaTrainSync.Domain.Entities.EntrenamientosZonas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Infrastructure.Persistence.Configurations.EntrenamientoZonas
{
    public class ZonaEntrenamientoConfiguration : IEntityTypeConfiguration<ZonaEntrenamientoEnt>
    {
        public void Configure(EntityTypeBuilder<ZonaEntrenamientoEnt> builder)
        {
            builder.ToTable("ZonaEntrenamiento");

            // Primary Key
            builder.HasKey(z => z.IdZona);
            builder.Property(z => z.IdZona)
                .ValueGeneratedOnAdd();

            // Propiedades
            builder.Property(z => z.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(z => z.Descripcion)
                .HasMaxLength(255);

            builder.Property(z => z.Factor)
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            // Relaciones
            builder.HasMany(z => z.DetalleZonaPlanes)
                .WithOne(d => d.Zona)
                .HasForeignKey(d => d.IdZona)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
