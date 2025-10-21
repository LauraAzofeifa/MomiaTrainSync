using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Models;

namespace MomiaTrainSync.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<EntrenadorAtleta> EntrenadoresAtletas { get; set; }
        public DbSet<PlanEntrenamiento> PlanesEntrenamiento { get; set; }
        public DbSet<Objetivo> Objetivos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EntrenadorAtleta>()
                .HasOne(ea => ea.Entrenador)
                .WithMany()
                .HasForeignKey(ea => ea.EntrenadorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EntrenadorAtleta>()
                .HasOne(ea => ea.Atleta)
                .WithMany()
                .HasForeignKey(ea => ea.AtletaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EntrenadorAtleta>()
                .HasIndex(ea => new { ea.EntrenadorId, ea.AtletaId })
                .IsUnique();

            modelBuilder.Entity<PlanEntrenamiento>()
                .HasOne(p => p.Atleta)
                .WithMany()
                .HasForeignKey(p => p.IdAtleta)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlanEntrenamiento>()
                .HasOne(p => p.Creador)
                .WithMany()
                .HasForeignKey(p => p.IdCreador)
                .OnDelete(DeleteBehavior.Restrict);


            DbInitializer.Seed(modelBuilder);
        }
    }
}
