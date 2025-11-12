using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles
{
    public interface IPermisoRepository
    {
        // Consultas
        Task<List<PermisoEnt>> GetAllAsync(bool incluirInactivos = false);
        Task<PermisoEnt?> GetByIdAsync(int id);
        Task<PermisoEnt?> GetByCodigoAsync(string codigo);
        Task<bool> HasPermissionAsync(int userId, string permissionCode);

        // CRUD
        Task<PermisoEnt?> AddAsync(PermisoEnt permiso);
        Task<bool> UpdateAsync(PermisoEnt permiso);
        Task<bool> DeleteAsync(int id); // puede ser soft delete

        // Utilidades
        Task<List<PermisoEnt>> GetByCategoriaAsync(string categoria);
    }
}
