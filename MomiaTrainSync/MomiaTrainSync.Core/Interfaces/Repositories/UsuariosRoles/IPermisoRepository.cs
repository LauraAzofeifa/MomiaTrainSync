using MomiaTrainSync.Core.Interfaces.Repositories.Base;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles
{
    public interface IPermisoRepository : IGenericRepository<PermisoEnt>
    {
        Task<List<PermisoEnt>> GetByCategoriaAsync(string categoria);
        Task<bool> HasPermissionAsync(int userId, string route);
        Task<PermisoEnt?> GetByCodigoAsync(string codigo);
        Task<PermisoEnt?> GetByRutaAsync(string ruta);
    }
}
