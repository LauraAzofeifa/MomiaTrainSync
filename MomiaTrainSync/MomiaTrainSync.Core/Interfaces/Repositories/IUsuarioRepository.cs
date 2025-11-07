using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;

namespace MomiaTrainSync.Core.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<UsuarioEnt?> GetByEmailAsync(string email);
        Task<UsuarioEnt?> GetByIdAsync(int id);
        Task<UsuarioEnt?> AddAsync(UsuarioEnt usuario);
        Task<bool> UpdateAsync(UsuarioEnt usuario);
        Task DeleteAsync(int id);

        // Listas
        Task<List<UsuarioEnt>> GetAllAsync(bool incluirInactivos = false);

        Task<List<UsuarioEnt>> GetAtletasByEntrenadorAsync(int entrenadorId, bool incluirInactivos = false);
    }
}
