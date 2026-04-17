using MomiaTrainSync.Core.Interfaces.Repositories.EntrenadorAtleta;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Domain.Entities.EntrenamientosZonas;
using MomiaTrainSync.Infrastructure.Persistence;
using MomiaTrainSync.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Infrastructure.Repositories.EntrenamientosZonas
{
    public class DetalleZonaPlanRepository : GenericRepository<DetalleZonaPlanEnt>, IDetalleZonaPlanRepository
    {
        public DetalleZonaPlanRepository(
            MomiaTrainSyncDbContext context, 
            ILogErrorRepository logger
            ) : base(context, logger)
        {
        }
    }
}
