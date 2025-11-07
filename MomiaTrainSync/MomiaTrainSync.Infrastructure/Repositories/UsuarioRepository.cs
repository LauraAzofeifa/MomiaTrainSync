using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Core.Interfaces.Repositories;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using MomiaTrainSync.Infrastructure.Persistence;
using System;

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

        public async Task<List<UsuarioEnt>> GetAllAsync(bool incluirInactivos = false)
        {
            try
            {
                var query = _context.Usuarios
                                     .Include(u => u.Rol)
                                     .AsNoTracking();

                query = !incluirInactivos
                    ? query.Where(u => u.Estado)
                    : query;

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(UsuarioRepository)}.{nameof(GetAllAsync)}", ex);
                return new List<UsuarioEnt>();
            }
        }

        public async Task<List<UsuarioEnt>> GetAtletasByEntrenadorAsync(int entrenadorId, bool incluirInactivos = false)
        {
            try
            {
                var query = _context.EntrenadorAtletas
                    .Where(ea => ea.IdEntrenador == entrenadorId)
                    .Include(ea => ea.Atleta)
                        .ThenInclude(a => a!.Rol)
                    .AsNoTracking()
                    .Select(ea => ea.Atleta);

                query = !incluirInactivos
                    ? query.Where(a => a != null && a.Estado)
                    : query;

                // Filter out nulls to match List<UsuarioEnt> (not List<UsuarioEnt?>)
                return await query.Where(a => a != null).Select(a => a!).ToListAsync();
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(UsuarioRepository)}.{nameof(GetAtletasByEntrenadorAsync)}", ex);
                return new List<UsuarioEnt>();
            }
        }
    }
}
