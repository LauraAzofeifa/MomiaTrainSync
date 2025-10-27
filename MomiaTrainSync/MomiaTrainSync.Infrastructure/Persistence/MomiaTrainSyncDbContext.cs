using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Domain.Entities;
using System;

namespace MomiaTrainSync.Infrastructure.Persistence
{
    public class MomiaTrainSyncDbContext : DbContext
    {
        // Constructor
        public MomiaTrainSyncDbContext(DbContextOptions<MomiaTrainSyncDbContext> options)
            : base(options) { }

        // DbSets
        public DbSet<UsuarioEnt> Usuarios { get; set; }
        public DbSet<RolEnt> Roles { get; set; }
        public DbSet<PermisoEnt> Permisos { get; set; }
        public DbSet<RolPermisoEnt> RolesPermisos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones de ensamblado
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MomiaTrainSyncDbContext).Assembly);
        }
    }
}
