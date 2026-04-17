using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Domain.Entities;
using MomiaTrainSync.Domain.Entities.EntrenadorAtleta;
using MomiaTrainSync.Domain.Entities.EntrenamientosZonas;
using MomiaTrainSync.Domain.Entities.RutinasAsignaciones;
using MomiaTrainSync.Domain.Entities.RutinasEntrenamientos;
using MomiaTrainSync.Domain.Entities.SesionesEntrenamiento;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;

namespace MomiaTrainSync.Infrastructure.Persistence
{
    public class MomiaTrainSyncDbContext : DbContext
    {
        // Constructor
        public MomiaTrainSyncDbContext(DbContextOptions<MomiaTrainSyncDbContext> options)
            : base(options) { }

        #region DbSets

        #region UsuariosRoles
        public DbSet<PermisoEnt> Permisos { get; set; }
        public DbSet<RolEnt> Roles { get; set; }
        public DbSet<RolPermisoEnt> RolesPermisos { get; set; }
        public DbSet<UsuarioEnt> Usuarios { get; set; }
        #endregion

        #region EntrenadorAtleta
        public DbSet<EntrenadorAtletaEnt> EntrenadorAtletas { get; set; }
        #endregion

        #region RutinasEntrenamientos
        public DbSet<RutinaEnt> Rutinas { get; set; }
        public DbSet<EntrenamientoEnt> Entrenamientos { get; set; }
        public DbSet<TipoSesionEnt> TiposSesion { get; set; }
        #endregion

        #region EntrenamientosZonas
        public DbSet<ZonaEntrenamientoEnt> ZonasEntrenamiento { get; set; }
        public DbSet<DetalleZonaPlanEnt> DetallesZonasPlan { get; set; }
        #endregion

        #region SesionesEntrenamiento
        public DbSet<SesionEntrenamientoEnt> SesionesEntrenamiento { get; set; }
        public DbSet<DetalleZonaSesionEnt> DetallesZonasSesion { get; set; }
        #endregion

        #region Logs
        public DbSet<LogErrorEnt> LogErrores { get; set; }
        #endregion

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones de ensamblado
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MomiaTrainSyncDbContext).Assembly);
        }
    }
}
