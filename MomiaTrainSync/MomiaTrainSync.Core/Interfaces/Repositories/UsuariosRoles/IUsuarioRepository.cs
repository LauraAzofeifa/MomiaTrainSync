using MomiaTrainSync.Core.Interfaces.Repositories.Base;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles
{
    public interface IUsuarioRepository : IGenericRepository<UsuarioEnt>
    {
        Task<UsuarioEnt?> GetByEmailAsync(string email);
        Task<UsuarioEnt?> GetByIdWithRolAsync(int id);
        Task<List<UsuarioEnt>> GetAtletasByEntrenadorAsync(
            int entrenadorId,
            bool includeInactive = false);
    }
}
