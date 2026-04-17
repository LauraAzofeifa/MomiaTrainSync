using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.RutinasEntrenamientos;
using MomiaTrainSync.Domain.Entities.RutinasAsignaciones;
using MomiaTrainSync.Infrastructure.Persistence;
using MomiaTrainSync.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace MomiaTrainSync.Infrastructure.Repositories.RutinasEntrenamientos
{
    public class RutinaRepository
        : GenericRepository<RutinaEnt>, IRutinaRepository
    {
        public RutinaRepository(
            MomiaTrainSyncDbContext context,
            ILogErrorRepository logger
        ) : base(context, logger)
        {
        }

        public async Task<List<RutinaEnt>> GetRutinasAsync(
            int? idRutina,
            int? idRelacion,
            bool incluirInactivos)
        {
            // Partimos del GetAllAsync existente para aprovechar include, tracking, etc.
            var query = await GetAllAsync(
                asNoTracking: true,
                include: null,
                includeInactive: incluirInactivos
            );

            // Convertimos a IQueryable para poder filtrar
            var q = query.AsQueryable();

            if (idRutina.HasValue)
            {
                q = q.Where(r => r.IdRutina == idRutina.Value);
            }

            if (idRelacion.HasValue)
            {
                q = q.Where(r => r.IdRelacion == idRelacion.Value);
            }

            return q.ToList();
        }

        public async Task<List<RutinaEnt>> GetByRelacionAsync(
            int idRelacion,
            bool incluirInactivos = false)
        {
            return await GetAllAsync(
                include: q => q
                    .Include(x => x.Entrenamientos)
                    .Where(x => x.IdRelacion == idRelacion),
                includeInactive: incluirInactivos
            );
        }

        public async Task<bool> ExisteNombreAsync(
            int idRelacion,
            string nombre,
            int? ignorarId = null)
        {
            return await FirstAsync(
                x =>
                    x.IdRelacion == idRelacion &&
                    x.Nombre == nombre &&
                    (!ignorarId.HasValue || x.IdRutina != ignorarId.Value)
            ) != null;
        }

        // Contamos las rutinas de una relación activas
        public async Task<int> ContarRutinasActivasAsync(int? idRelacion, bool trainer = false, bool todas = false)
        {
            // Si se piden todas, las contamos sin filtro
            if (todas)
            {
                return await CountAsync(
                    x => x.Estado
                );
            }

            // Si es trainer, contamos todas las rutinas activas de sus atletas
            if (trainer)
            {
                return await CountAsync(
                    x => x.Estado && x.Relacion != null && x.Relacion.Entrenador != null
                );
            }

            return await CountAsync(
                x => x.IdRelacion == idRelacion && x.Estado
            );
        }
    }
}
