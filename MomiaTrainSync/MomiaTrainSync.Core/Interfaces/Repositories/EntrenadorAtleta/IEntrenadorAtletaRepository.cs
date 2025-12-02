using MomiaTrainSync.Core.Interfaces.Repositories.Base;
using MomiaTrainSync.Domain.Entities.EntrenadorAtleta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.Interfaces.Repositories.EntrenadorAtleta
{
    public interface IEntrenadorAtletaRepository : IGenericRepository<EntrenadorAtletaEnt>
    {
        Task<EntrenadorAtletaEnt?> AsignarRelacionAsync(EntrenadorAtletaEnt relacion);
        Task<List<EntrenadorAtletaEnt>> GetByEntrenadorAsync(int entrenadorId, bool incluirInactivos = false);
        Task<List<EntrenadorAtletaEnt>> GetByAtletaAsync(int atletaId, bool incluirInactivos = false);

    }
}
