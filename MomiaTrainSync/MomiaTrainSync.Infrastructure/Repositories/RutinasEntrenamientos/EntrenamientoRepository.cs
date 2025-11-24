using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.RutinasEntrenamientos;
using MomiaTrainSync.Domain.Entities.EntrenamientosZonas;
using MomiaTrainSync.Infrastructure.Persistence;
using MomiaTrainSync.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Infrastructure.Repositories.RutinasEntrenamientos
{
    public class EntrenamientoRepository : GenericRepository<EntrenamientoEnt>, IEntrenamientoRepository
    {
        public EntrenamientoRepository(
            MomiaTrainSyncDbContext context,
            ILogErrorRepository loggerError
            ) : base( context, loggerError )
        {
            
        }

        public async Task<List<EntrenamientoEnt>> GetByRutinaAsync(int idRutina)
        {
            return await GetAllAsync(
                include: q => q.Where(rt => rt.IdRutina == idRutina)    
            );
        }
    }
}
