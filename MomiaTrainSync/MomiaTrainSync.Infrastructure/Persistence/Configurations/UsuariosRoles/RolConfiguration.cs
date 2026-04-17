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

            builder.Property(r => r.Estado)
                .IsRequired();

            // Índices y unicidad
            builder.HasIndex(r => r.Nombre).IsUnique();

            // Relaciones
            builder.HasMany(r => r.Usuarios)
                .WithOne(u => u.Rol)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(r => r.RolPermisos)
                .WithOne(rp => rp.Rol)
                .HasForeignKey(rp => rp.IdRol)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
