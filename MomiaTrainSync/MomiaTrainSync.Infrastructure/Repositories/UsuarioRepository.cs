using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Core.Interfaces.Repositories;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Domain.Entities;
using MomiaTrainSync.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly MomiaTrainSyncDbContext _context;
        private readonly ILogErrorRepository _logErrorRepository;

        public UsuarioRepository(MomiaTrainSyncDbContext context, ILogErrorRepository logErrorRepository)
        {
            _context = context;
            _logErrorRepository = logErrorRepository;
        }

        public async Task<UsuarioEnt?> GetByEmailAsync(string email)
        {
            // Métodos simples: sin try/catch
            return await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Correo == email);
        }

        public async Task<UsuarioEnt?> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UsuarioEnt?> AddAsync(UsuarioEnt usuario)
        {
            try
            {
                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();
                return usuario;
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(UsuarioRepository)}.{nameof(AddAsync)}", ex);
                throw; // Propaga la excepción para que el UseCase decida qué hacer
            }
        }

        public async Task<bool> UpdateAsync(UsuarioEnt usuario)
        {
            try
            {
                _context.Usuarios.Update(usuario);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(UsuarioRepository)}.{nameof(UpdateAsync)}", ex);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null) return;

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(UsuarioRepository)}.{nameof(DeleteAsync)}", ex);
                throw;
            }
        }

        public async Task<List<UsuarioEnt>> GetAllAsync()
        {
            try
            {
                return await _context.Usuarios
                                     .Include(u => u.Rol)
                                     .AsNoTracking()
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(UsuarioRepository)}.{nameof(GetAllAsync)}", ex);
                return new List<UsuarioEnt>();
            }
        }
    }
}
