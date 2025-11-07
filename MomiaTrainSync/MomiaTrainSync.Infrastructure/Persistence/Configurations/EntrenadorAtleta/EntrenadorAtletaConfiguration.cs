using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MomiaTrainSync.Domain.Entities.EntrenadorAtleta;
using System;

namespace MomiaTrainSync.Infrastructure.Persistence.Configurations.EntrenadorAtleta
{
    public class EntrenadorAtletaConfiguration : IEntityTypeConfiguration<EntrenadorAtletaEnt>
    {
        public void Configure(EntityTypeBuilder<EntrenadorAtletaEnt> builder)
        {
            builder.ToTable("EntrenadorAtleta");

            // Clave primaria
            builder.HasKey(ea => ea.IdRelacion);
            builder.Property(ea => ea.IdRelacion)
                .ValueGeneratedOnAdd();

            // Propiedades
            builder.Property(ea => ea.FechaAsignacion)
                .IsRequired();

            builder.Property(ea => ea.Estado)
                .IsRequired();

            // Relaciones
            builder.HasOne(ea => ea.Entrenador)
                .WithMany() // Un entrenador puede tener muchos atletas
                .HasForeignKey(ea => ea.IdEntrenador)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_EntrenadorAtleta_Entrenador");

            builder.HasOne(ea => ea.Atleta)
                .WithMany() // Un atleta puede estar asignado a un entrenador
                .HasForeignKey(ea => ea.IdAtleta)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_EntrenadorAtleta_Atleta");

            builder.HasMany(ea => ea.Asignaciones)
                .WithOne(a => a.Relacion)
                .HasForeignKey(a => a.IdRelacion)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
