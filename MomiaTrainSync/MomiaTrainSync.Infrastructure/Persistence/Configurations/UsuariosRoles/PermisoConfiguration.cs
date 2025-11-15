using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;

namespace MomiaTrainSync.Infrastructure.Persistence.Configurations.UsuariosRoles
{
    public class PermisoConfiguration : IEntityTypeConfiguration<PermisoEnt>
    {
        public void Configure(EntityTypeBuilder<PermisoEnt> builder)
        {
            builder.ToTable("Permiso");

            // Primary Key
            builder.HasKey(p => p.IdPermiso);
            builder.Property(p => p.IdPermiso)
                .ValueGeneratedOnAdd();

            // Propiedades
            builder.Property(p => p.Codigo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Descripcion)
                .HasMaxLength(255);

            builder.Property(p => p.Categoria)
                .HasMaxLength(100);

            builder.Property(p => p.Ruta)
                .HasMaxLength(200);

            builder.Property(p => p.Estado)
                .IsRequired();

            // Relaciones
            builder.HasMany(p => p.RolPermisos)
                .WithOne(rp => rp.Permiso)
                .HasForeignKey(rp => rp.IdPermiso)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
