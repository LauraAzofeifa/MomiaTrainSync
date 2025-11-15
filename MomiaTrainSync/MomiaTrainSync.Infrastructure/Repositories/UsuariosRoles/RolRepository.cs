using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using MomiaTrainSync.Infrastructure.Persistence;

namespace MomiaTrainSync.Infrastructure.Repositories.UsuariosRoles
{
    public class RolRepository : IRolRepository
    {
        private readonly MomiaTrainSyncDbContext _context;
        private readonly ILogErrorRepository _logErrorRepository;

        public RolRepository(MomiaTrainSyncDbContext context, ILogErrorRepository logErrorRepository)
        {
            _context = context;
            _logErrorRepository = logErrorRepository;
        }

        #region === CRUD Roles ===

        public async Task<List<RolEnt>> GetAllAsync()
        {
            try
            {
                return await _context.Roles
                    .Include(r => r.RolPermisos)
                        .ThenInclude(rp => rp.Permiso)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(RolRepository)}.{nameof(GetAllAsync)}", ex);
                return new List<RolEnt>();
            }
        }

        public async Task<RolEnt?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Roles
                    .Include(r => r.RolPermisos)
                        .ThenInclude(rp => rp.Permiso)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.IdRol == id);
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(RolRepository)}.{nameof(GetByIdAsync)}", ex);
                return null;
            }
        }

        public async Task<RolEnt?> GetByNombreAsync(string nombre)
        {
            try
            {
                return await _context.Roles
                    .FirstOrDefaultAsync(r => r.Nombre == nombre);
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(RolRepository)}.{nameof(GetByNombreAsync)}", ex);
                return null;
            }
        }

        public async Task<RolEnt?> AddAsync(RolEnt rol)
        {
            try
            {
                await _context.Roles.AddAsync(rol);
                await _context.SaveChangesAsync();
                return rol;
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(RolRepository)}.{nameof(AddAsync)}", ex);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(RolEnt rol)
        {
            try
            {
                _context.Roles.Update(rol);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(RolRepository)}.{nameof(UpdateAsync)}", ex);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var rol = await _context.Roles.FindAsync(id);
                if (rol == null)
                    return false;

                _context.Roles.Remove(rol);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(RolRepository)}.{nameof(DeleteAsync)}", ex);
                return false;
            }
        }

        #endregion

        #region === Relaciones con Permisos ===

        public async Task<List<PermisoEnt>> GetPermisosPorRolAsync(int idRol)
        {
            try
            {
                return await _context.RolesPermisos
                    .Where(rp => rp.IdRol == idRol)
                    .Include(rp => rp.Permiso)
                    .Select(rp => rp.Permiso!)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(RolRepository)}.{nameof(GetPermisosPorRolAsync)}", ex);
                return new List<PermisoEnt>();
            }
        }

        public async Task<bool> AsignarPermisosAsync(int idRol, IEnumerable<int> permisosIds)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Eliminar los actuales
                var actuales = _context.RolesPermisos.Where(rp => rp.IdRol == idRol);
                _context.RolesPermisos.RemoveRange(actuales);

                // Agregar nuevos
                var nuevos = permisosIds.Select(pid => new RolPermisoEnt { IdRol = idRol, IdPermiso = pid });
                await _context.RolesPermisos.AddRangeAsync(nuevos);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await _logErrorRepository.AddLogAsync($"{nameof(RolRepository)}.{nameof(AsignarPermisosAsync)}", ex);
                return false;
            }
        }

        #endregion
    }
}
