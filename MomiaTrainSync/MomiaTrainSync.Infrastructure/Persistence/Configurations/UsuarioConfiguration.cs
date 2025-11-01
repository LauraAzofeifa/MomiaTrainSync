using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MomiaTrainSync.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Infrastructure.Persistence.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<UsuarioEnt>
    {
        public void Configure(EntityTypeBuilder<UsuarioEnt> builder)
        {
            builder.ToTable("Usuario");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Apellido)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Correo)
               .IsRequired()
               .HasMaxLength(150);

            builder.Property(u => u.ContrasennaHash)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Estado)
                .HasDefaultValue(true);

            builder.Property(u => u.FechaIngreso)
                .HasColumnType("datetime");

            // Relacion Rol 1:M
            builder.HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
