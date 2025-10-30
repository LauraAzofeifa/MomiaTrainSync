using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MomiaTrainSync.Domain.Entities;

namespace MomiaTrainSync.Infrastructure.Persistence.Configurations
{
    public class RolConfiguration : IEntityTypeConfiguration<RolEnt>
    {
        public void Configure(EntityTypeBuilder<RolEnt> builder)
        {
            builder.ToTable("Rol");
            builder.HasKey(r => r.IdRol);

            builder.Property(r => r.IdRol)
                .ValueGeneratedOnAdd();

            builder.Property(r => r.IdRol)
                .ValueGeneratedOnAdd();

            builder.Property(r => r.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Descripcion)
                .HasMaxLength(255);

            // Seed inicial de Roles (Admin, Entrenador, Atleta)
            builder.HasData(
                new RolEnt
                {
                    IdRol = -1,
                    Nombre = "Administrador",
                    Descripcion = "Acceso completo al sistema"
                },
                new RolEnt
                {
                    IdRol = -2,
                    Nombre = "Entrenador",
                    Descripcion = "Gestión de entrenamientos y seguimiento de atletas"
                },
                new RolEnt
                {
                    IdRol = -3,
                    Nombre = "Atleta",
                    Descripcion = "Acceso básico a funcionalidades del sistema"
                }
            );
        }
    }

    public class PermisoConfiguration : IEntityTypeConfiguration<PermisoEnt>
    {
        public void Configure(EntityTypeBuilder<PermisoEnt> builder)
        {
            builder.ToTable("Permiso");

            builder.HasKey(p => p.IdPermiso);

            builder.Property(p => p.IdPermiso)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.IdPermiso)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Codigo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Estado)
                .HasDefaultValue(true);

            // Seed inicial de Permisos
            builder.HasData(
                new PermisoEnt { IdPermiso = -1, Codigo = "GESTIONAR_USUARIOS", Estado = true },
                new PermisoEnt { IdPermiso = -2, Codigo = "GESTIONAR_ENTRENAMIENTOS", Estado = true },
                new PermisoEnt { IdPermiso = -3, Codigo = "GESTIONAR_REPORTES", Estado = true },
                new PermisoEnt { IdPermiso = -4, Codigo = "GESTIONAR_PERFILES", Estado = true },
                new PermisoEnt { IdPermiso = -5, Codigo = "GESTIONAR_RUTINAS", Estado = true }
            );
        }
    }

    public class RolPermisoConfiguration : IEntityTypeConfiguration<RolPermisoEnt>
    {
        public void Configure(EntityTypeBuilder<RolPermisoEnt> builder)
        {
            builder.ToTable("RolPermiso");

            builder.HasKey(rp => new { rp.IdRol, rp.IdPermiso });

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
                new RolPermisoEnt { IdRol = -1, IdPermiso = -1 },
                new RolPermisoEnt { IdRol = -1, IdPermiso = -2 },
                new RolPermisoEnt { IdRol = -1, IdPermiso = -3 },
                new RolPermisoEnt { IdRol = -1, IdPermiso = -4 },
                new RolPermisoEnt { IdRol = -1, IdPermiso = -5 },

                new RolPermisoEnt { IdRol = -2, IdPermiso = -2 },
                new RolPermisoEnt { IdRol = -2, IdPermiso = -3 },
                new RolPermisoEnt { IdRol = -2, IdPermiso = -4 },
                new RolPermisoEnt { IdRol = -2, IdPermiso = -5 },

                new RolPermisoEnt { IdRol = -3, IdPermiso = -4 },
                new RolPermisoEnt { IdRol = -3, IdPermiso = -5 }
            );
        }
    }
}
