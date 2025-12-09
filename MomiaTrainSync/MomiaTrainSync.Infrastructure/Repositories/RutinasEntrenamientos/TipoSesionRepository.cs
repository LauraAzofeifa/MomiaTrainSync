using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.RutinasEntrenamientos;
using MomiaTrainSync.Domain.Entities.RutinasEntrenamientos;
using MomiaTrainSync.Infrastructure.Persistence;
using MomiaTrainSync.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Infrastructure.Repositories.RutinasEntrenamientos
{
    public class TipoSesionRepository : GenericRepository<TipoSesionEnt>, ITipoSesionRepository
    {
        public TipoSesionRepository(
            MomiaTrainSyncDbContext context, 
            ILogErrorRepository logger
            ) : base(context, logger)
        {
        }
    }
}
