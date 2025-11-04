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
                .HasDefaultValue(true);

            // Seed inicial de Permisos
            builder.HasData(
                // ADMIN PERMISOS
                new PermisoEnt
                {
                    IdPermiso = -1,
                    Codigo = "GESTIONAR_USUARIOS",
                    Descripcion = "Permite administrar usuarios del sistema",
                    Categoria = "Admin",
                    Ruta = "/Admin/ManageUsers",
                    Estado = true
                },
                // ENTRENADOR PERMISOS
                new PermisoEnt
                {
                    IdPermiso = -2,
                    Codigo = "GESTIONAR_ATLETAS",
                    Descripcion = "Permite gestionar atletas",
                    Categoria = "Trainer",
                    Ruta = "/Trainer/ManageAthletes",
                    Estado = true
                },
                // PERFIL PERMISOS
                new PermisoEnt
                {
                    IdPermiso = -3,
                    Codigo = "VER_PERFIL",
                    Descripcion = "Permite ver el perfil del usuario",
                    Categoria = "Profile",
                    Ruta = "/Profile/MyProfile",
                    Estado = true
                },
                new PermisoEnt
                {
                    IdPermiso = -4,
                    Codigo = "EDITAR_PERFIL",
                    Descripcion = "Permite editar el perfil del usuario",
                    Categoria = "Profile",
                    Ruta = "/Profile/EditProfile",
                    Estado = true
                },
                new PermisoEnt
                {
                    IdPermiso = -5,
                    Codigo = "CAMBIAR_CONTRASENNA_PERFIL",
                    Descripcion = "Permite cambiar la contraseña del usuario",
                    Categoria = "Profile",
                    Ruta = "/Profile/ChangePassword",
                    Estado = true
                }

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
                // ADMIN: todos los permisos
                new RolPermisoEnt { IdRol = -1, IdPermiso = -1 },
                new RolPermisoEnt { IdRol = -1, IdPermiso = -2 },
                new RolPermisoEnt { IdRol = -1, IdPermiso = -3 },
                new RolPermisoEnt { IdRol = -1, IdPermiso = -4 },
                new RolPermisoEnt { IdRol = -1, IdPermiso = -5 },

                // TRAINER: solo los suyos + perfil
                new RolPermisoEnt { IdRol = -2, IdPermiso = -2 },
                new RolPermisoEnt { IdRol = -2, IdPermiso = -3 },
                new RolPermisoEnt { IdRol = -2, IdPermiso = -4 },
                new RolPermisoEnt { IdRol = -2, IdPermiso = -5 },

                // ATHLETE: solo perfil
                new RolPermisoEnt { IdRol = -3, IdPermiso = -3 },
                new RolPermisoEnt { IdRol = -3, IdPermiso = -4 },
                new RolPermisoEnt { IdRol = -3, IdPermiso = -5 }
            );
        }
    }
}
