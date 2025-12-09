using MomiaTrainSync.Core.Interfaces.Repositories.EntrenamientosZonas;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Domain.Entities.EntrenamientosZonas;
using MomiaTrainSync.Infrastructure.Persistence;
using MomiaTrainSync.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Infrastructure.Repositories.EntrenamientosZonas
{
    public class ZonaEntrenamientoRepository : GenericRepository<ZonaEntrenamientoEnt>, IZonaEntrenamientoRepository
    {
        public ZonaEntrenamientoRepository(
            MomiaTrainSyncDbContext context, 
            ILogErrorRepository logger
            ) : base(context, logger)
        {
        }
    }
}
