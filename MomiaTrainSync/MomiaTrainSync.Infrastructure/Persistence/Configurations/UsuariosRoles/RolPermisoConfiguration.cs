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

            // Seed inicial de RolesPermisos
            builder.HasData(
                // ADMIN: todos los permisos
                new RolPermisoEnt { IdRol = -1, IdPermiso = -1 },
                new RolPermisoEnt { IdRol = -1, IdPermiso = -2 },
                new RolPermisoEnt { IdRol = -1, IdPermiso = -3 },
                new RolPermisoEnt { IdRol = -1, IdPermiso = -4 },
                new RolPermisoEnt { IdRol = -1, IdPermiso = -5 },
                new RolPermisoEnt { IdRol = -1, IdPermiso = -8 },

                // TRAINER: solo los suyos + perfil
                new RolPermisoEnt { IdRol = -2, IdPermiso = -2 },
                new RolPermisoEnt { IdRol = -2, IdPermiso = -3 },
                new RolPermisoEnt { IdRol = -2, IdPermiso = -4 },
                new RolPermisoEnt { IdRol = -2, IdPermiso = -5 },
                new RolPermisoEnt { IdRol = -2, IdPermiso = -6 }, 
                new RolPermisoEnt { IdRol = -2, IdPermiso = -7 },

                // ATHLETE: solo perfil
                new RolPermisoEnt { IdRol = -3, IdPermiso = -3 },
                new RolPermisoEnt { IdRol = -3, IdPermiso = -4 },
                new RolPermisoEnt { IdRol = -3, IdPermiso = -5 }
            );
        }
    }
}
