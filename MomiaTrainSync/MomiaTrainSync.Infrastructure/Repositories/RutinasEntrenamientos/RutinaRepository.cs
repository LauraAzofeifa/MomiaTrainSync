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
    }
}
