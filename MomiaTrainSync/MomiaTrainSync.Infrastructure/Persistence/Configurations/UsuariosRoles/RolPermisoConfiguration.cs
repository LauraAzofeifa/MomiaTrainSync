using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;

namespace MomiaTrainSync.Infrastructure.Persistence.Configurations.UsuariosRoles
{
    public class RolPermisoConfiguration : IEntityTypeConfiguration<RolPermisoEnt>
    {
        public void Configure(EntityTypeBuilder<RolPermisoEnt> builder)
        {
            builder.ToTable("RolPermiso");

            // Clave compuesta
            builder.HasKey(rp => new { rp.IdRol, rp.IdPermiso });

            // Relaciones
            builder.HasOne(rp => rp.Rol)
                .WithMany(r => r.RolPermisos)
                .HasForeignKey(rp => rp.IdRol)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rp => rp.Permiso)
                .WithMany(p => p.RolPermisos)
                .HasForeignKey(rp => rp.IdPermiso)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
