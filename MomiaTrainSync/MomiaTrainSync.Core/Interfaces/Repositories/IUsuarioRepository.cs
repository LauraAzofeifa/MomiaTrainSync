using MomiaTrainSync.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<UsuarioEnt?> GetByEmailAsync(string email);
        Task<UsuarioEnt?> GetByIdAsync(int id);
        Task<UsuarioEnt?> AddAsync(UsuarioEnt usuario);
        Task UpdateAsync(UsuarioEnt usuario);
        Task DeleteAsync(int id);

        // Listas
        Task <List<UsuarioEnt>> GetAllAsync();
    }
}
