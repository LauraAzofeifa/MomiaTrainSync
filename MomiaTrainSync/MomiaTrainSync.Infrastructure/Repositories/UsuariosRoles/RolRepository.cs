using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using MomiaTrainSync.Infrastructure.Persistence;
using MomiaTrainSync.Infrastructure.Repositories.Base;

namespace MomiaTrainSync.Infrastructure.Repositories.UsuariosRoles
{
    public class RolRepository
        : GenericRepository<RolEnt>, IRolRepository
    {
        public RolRepository(
            MomiaTrainSyncDbContext context,
            ILogErrorRepository logger)
            : base(context, logger)
        {
        }

        #region === CRUD Roles ===

        public async Task<List<RolEnt>> GetAllWithPermisosAsync()
        {
            try
            {
                return await GetAllAsync(
                    include: q => q
                        .Include(r => r.RolPermisos)
                        .ThenInclude(rp => rp.Permiso)
                );
            }
            catch (Exception ex)
            {
                await _logger.AddLogAsync(
                    $"{nameof(RolRepository)}.{nameof(GetAllWithPermisosAsync)}", ex);
                return new List<RolEnt>();
            }
        }

        public async Task<RolEnt?> GetByIdWithPermisosAsync(int id)
        {
            try
            {
                return await FirstAsync(
                    r => r.IdRol == id,
                    include: q => q
                        .Include(r => r.RolPermisos)
                        .ThenInclude(rp => rp.Permiso)
                );
            }
            catch (Exception ex)
            {
                await _logger.AddLogAsync(
                    $"{nameof(RolRepository)}.{nameof(GetByIdWithPermisosAsync)}", ex);
                return null;
            }
        }

        public async Task<RolEnt?> GetByNombreAsync(string nombre)
        {
            try
            {
                return await FirstAsync(
                    r => r.Nombre == nombre
                );
            }
            catch (Exception ex)
            {
                await _logger.AddLogAsync(
                    $"{nameof(RolRepository)}.{nameof(GetByNombreAsync)}", ex);
                return null;
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
                await _logger.AddLogAsync(
                    $"{nameof(RolRepository)}.{nameof(GetPermisosPorRolAsync)}", ex);
                return new List<PermisoEnt>();
            }
        }

        public async Task<bool> AsignarPermisosAsync(int idRol, IEnumerable<int> permisosIds)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var actuales = _context.RolesPermisos
                    .Where(rp => rp.IdRol == idRol);

                _context.RolesPermisos.RemoveRange(actuales);

                var nuevos = permisosIds
                    .Select(pid => new RolPermisoEnt
                    {
                        IdRol = idRol,
                        IdPermiso = pid
                    });

                await _context.RolesPermisos.AddRangeAsync(nuevos);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await _logger.AddLogAsync(
                    $"{nameof(RolRepository)}.{nameof(AsignarPermisosAsync)}", ex);

                return false;
            }
        }

        #endregion
    }
}
