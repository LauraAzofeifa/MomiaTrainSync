using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles
{
    public interface IRolRepository
    {
        Task<List<RolEnt>> GetAllAsync();
        Task<RolEnt?> GetByIdAsync(int id);
        Task<RolEnt?> GetByNombreAsync(string nombre);
        Task<RolEnt?> AddAsync(RolEnt rol);
        Task<bool> UpdateAsync(RolEnt rol);
        Task<bool> DeleteAsync(int id);
        Task<List<PermisoEnt>> GetPermisosPorRolAsync(int idRol);
        Task<bool> AsignarPermisosAsync(int idRol, IEnumerable<int> permisosIds);
    }
}
