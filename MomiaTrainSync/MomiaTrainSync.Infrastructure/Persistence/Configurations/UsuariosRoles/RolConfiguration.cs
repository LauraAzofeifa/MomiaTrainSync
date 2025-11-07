using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;

namespace MomiaTrainSync.Infrastructure.Persistence.Configurations.UsuariosRoles
{
    public class RolConfiguration : IEntityTypeConfiguration<RolEnt>
    {
        public void Configure(EntityTypeBuilder<RolEnt> builder)
        {
            builder.ToTable("Rol");

            // Primary Key
            builder.HasKey(r => r.IdRol);
            builder.Property(r => r.IdRol)
                .ValueGeneratedOnAdd();

            // Propiedades
            builder.Property(r => r.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Descripcion)
                .HasMaxLength(255);

            // Relaciones
            builder.HasMany(r => r.Usuarios)
                .WithOne(u => u.Rol)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(r => r.RolPermisos)
                .WithOne(rp => rp.Rol)
                .HasForeignKey(rp => rp.IdRol)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed inicial de Roles (Admin, Entrenador, Atleta)
            builder.HasData(
                new RolEnt { IdRol = -1, Nombre = "Administrador", Descripcion = "Acceso completo al sistema" },
                new RolEnt { IdRol = -2, Nombre = "Entrenador", Descripcion = "Gestiona rutinas, entrenamientos y atletas" },
                new RolEnt { IdRol = -3, Nombre = "Atleta", Descripcion = "Usuario que recibe rutinas asignadas" }
            );
        }
    }
}
