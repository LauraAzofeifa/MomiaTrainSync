using MomiaTrainSync.Core.Interfaces.Repositories.Base;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles
{
    public interface IRolPermisoRepository : IGenericRepository<RolPermisoEnt>
    {
        Task<bool> ExistsAsync(int idRol, int idPermiso);
        Task<bool> DeleteAsync(int idRol, int idPermiso);
    }
}
