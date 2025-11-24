using MomiaTrainSync.Core.Interfaces.Repositories.Base;
using MomiaTrainSync.Domain.Entities.EntrenamientosZonas;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.Interfaces.Repositories.RutinasEntrenamientos
{
    public interface IEntrenamientoRepository : IGenericRepository<EntrenamientoEnt>
    {
        Task<List<EntrenamientoEnt>> GetByRutinaAsync(int idRutina);
    }
}
