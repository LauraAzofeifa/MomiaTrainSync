using MomiaTrainSync.Core.Interfaces.Repositories.Base;
using MomiaTrainSync.Domain.Entities.RutinasAsignaciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.Interfaces.Repositories.RutinasEntrenamientos
{
    public interface IRutinaRepository : IGenericRepository<RutinaEnt>
    {
        Task<List<RutinaEnt>> GetRutinasAsync(
            int? idRutina,
            int? idRelacion,
            bool incluirInactivos);
        Task<List<RutinaEnt>> GetByRelacionAsync(int idRelacion, bool incluirInactivos = false);
        Task<bool> ExisteNombreAsync(int idRelacion, string nombre, int? ignorarId = null);
        Task<int> ContarRutinasActivasAsync(int? idRelacion, bool trainer = false, bool todas = false);
    }
}
