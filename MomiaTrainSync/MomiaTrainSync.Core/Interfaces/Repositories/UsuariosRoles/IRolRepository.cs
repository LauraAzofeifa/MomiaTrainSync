using MomiaTrainSync.Core.Interfaces.Repositories.Base;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles
{
    public interface IRolRepository : IGenericRepository<RolEnt>
    {
        Task<RolEnt?> GetByNombreAsync(string nombre);
        Task<List<PermisoEnt>> GetPermisosPorRolAsync(int idRol);
        Task<bool> AsignarPermisosAsync(int idRol, IEnumerable<int> permisosIds);
    }
}
