using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;

namespace MomiaTrainSync.Infrastructure.Persistence.Configurations.UsuariosRoles
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<UsuarioEnt>
    {
        public void Configure(EntityTypeBuilder<UsuarioEnt> builder)
        {
            builder.ToTable("Usuarios");

            // Primary Key
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            // Propiedades básicas
            builder.Property(u => u.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Apellido)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Correo)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.Telefono)
                .HasMaxLength(20);

            builder.Property(u => u.Biografia)
                .HasMaxLength(255);

            builder.Property(u => u.FechaCumpleannos)
                .HasColumnType("datetime");

            builder.Property(u => u.ContrasennaHash)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Estado)
                .IsRequired();

            builder.Property(u => u.FechaCreacion)
                .IsRequired()
                .HasColumnType("datetime");

            // Relaciones
            builder.HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices y unicidad
            builder.HasIndex(u => u.Correo)
                .IsUnique();

            // Relaciones inversas
            builder.HasMany(u => u.EntrenamientosComoEntrenador)
                .WithOne(e => e.Entrenador)
                .HasForeignKey(e => e.IdEntrenador)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.EntrenamientosComoAtleta)
                .WithOne(e => e.Atleta)
                .HasForeignKey(e => e.IdAtleta)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
