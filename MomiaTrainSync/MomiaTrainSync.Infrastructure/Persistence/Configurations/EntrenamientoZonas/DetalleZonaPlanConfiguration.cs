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
    public class DetalleZonaPlanConfiguration : IEntityTypeConfiguration<DetalleZonaPlanEnt>
    {
        public void Configure(EntityTypeBuilder<DetalleZonaPlanEnt> builder)
        {
            builder.ToTable("DetalleZonaPlan");

            // Primary Key
            builder.HasKey(d => d.IdDetalleZonaPlan);
            builder.Property(d => d.IdDetalleZonaPlan)
                .ValueGeneratedOnAdd();

            // Propiedades
            builder.Property(d => d.MinutosPlanificados)
                .IsRequired();

            // Relaciones
            builder.HasOne(d => d.Entrenamiento)
                .WithMany(e => e.DetallesZonaPlan)
                .HasForeignKey(d => d.IdEntrenamiento)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Zona)
                .WithMany(z => z.DetalleZonaPlanes)
                .HasForeignKey(d => d.IdZona)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
