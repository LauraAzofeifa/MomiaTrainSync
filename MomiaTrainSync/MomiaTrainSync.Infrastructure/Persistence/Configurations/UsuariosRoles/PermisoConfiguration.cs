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
}
