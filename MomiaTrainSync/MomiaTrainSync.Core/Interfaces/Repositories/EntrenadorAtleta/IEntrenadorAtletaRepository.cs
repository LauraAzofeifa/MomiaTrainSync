using MomiaTrainSync.Domain.Entities.EntrenadorAtleta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.Interfaces.Repositories.EntrenadorAtleta
{
    public interface IEntrenadorAtletaRepository
    {
        Task<EntrenadorAtletaEnt?> GetByIdAsync(int id);
        Task<List<EntrenadorAtletaEnt>> GetAllAsync(bool incluirInactivos = false);
        Task<List<EntrenadorAtletaEnt>> GetByEntrenadorAsync(int entrenadorId, bool incluirInactivos = false);
        Task<List<EntrenadorAtletaEnt>> GetByAtletaAsync(int atletaId, bool incluirInactivos = false);
        Task<EntrenadorAtletaEnt?> AddAsync(EntrenadorAtletaEnt relacion);
        Task DeleteAsync(int id);
    }
}
