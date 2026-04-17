using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.SesionesEntrenamientos;
using MomiaTrainSync.Domain.Entities.SesionesEntrenamiento;
using MomiaTrainSync.Infrastructure.Persistence;
using MomiaTrainSync.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Infrastructure.Repositories.SesionesEntrenamientos
{
    public class DetalleZonaSesionRepository : GenericRepository<DetalleZonaSesionEnt>, IDetalleZonaSesionRepository
    {
        public DetalleZonaSesionRepository(
            MomiaTrainSyncDbContext context, 
            ILogErrorRepository logger
            ) : base(context, logger)
        {
        }
    }
}
