using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using MomiaTrainSync.Infrastructure.Persistence;
using System;

namespace MomiaTrainSync.Infrastructure.Repositories.RolesPermisos
{
    public class PermisoRepository : IPermisoRepository
    {
        private readonly MomiaTrainSyncDbContext _context;
        private readonly ILogErrorRepository _logErrorRepository;

        public PermisoRepository(MomiaTrainSyncDbContext context, ILogErrorRepository logErrorRepository)
        {
            _context = context;
            _logErrorRepository = logErrorRepository;
        }

        #region === Permisos: Consultas y CRUD ===

        public async Task<List<PermisoEnt>> GetAllAsync(bool incluirInactivos = false)
        {
            try
            {
                var query = _context.Permisos.AsNoTracking();
                if (!incluirInactivos)
                    query = query.Where(p => p.Estado);
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(PermisoRepository)}.{nameof(GetAllAsync)}", ex);
                return new List<PermisoEnt>();
            }
        }

        public async Task<PermisoEnt?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Permisos.AsNoTracking().FirstOrDefaultAsync(p => p.IdPermiso == id);
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(PermisoRepository)}.{nameof(GetByIdAsync)}", ex);
                return null;
            }
        }

        public async Task<PermisoEnt?> GetByCodigoAsync(string codigo)
        {
            try
            {
                return await _context.Permisos.AsNoTracking().FirstOrDefaultAsync(p => p.Codigo == codigo);
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(PermisoRepository)}.{nameof(GetByCodigoAsync)}", ex);
                return null;
            }
        }

        public async Task<PermisoEnt?> AddAsync(PermisoEnt permiso)
        {
            try
            {
                await _context.Permisos.AddAsync(permiso);
                await _context.SaveChangesAsync();
                return permiso;
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(PermisoRepository)}.{nameof(AddAsync)}", ex);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(PermisoEnt permiso)
        {
            try
            {
                _context.Permisos.Update(permiso);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(PermisoRepository)}.{nameof(UpdateAsync)}", ex);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var permiso = await _context.Permisos.FindAsync(id);
                if (permiso == null)
                    return false;

                // Soft delete (no se elimina físicamente)
                permiso.Estado = false;
                _context.Permisos.Update(permiso);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(PermisoRepository)}.{nameof(DeleteAsync)}", ex);
                return false;
            }
        }

        public async Task<List<PermisoEnt>> GetByCategoriaAsync(string categoria)
        {
            try
            {
                return await _context.Permisos
                    .Where(p => p.Categoria == categoria && p.Estado)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(PermisoRepository)}.{nameof(GetByCategoriaAsync)}", ex);
                return new List<PermisoEnt>();
            }
        }

        #endregion

        #region === Verificación de Permisos ===

        public async Task<bool> HasPermissionAsync(int userId, string route)
        {
            try
            {
                route = NormalizeRoute(route);

                var usuario = await _context.Usuarios
                    .AsNoTracking()
                    .Include(u => u.Rol)
                        .ThenInclude(r => r!.RolPermisos)
                            .ThenInclude(rp => rp.Permiso)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (usuario == null)
                    return false;

                // El rol Administrador tiene acceso a todo
                if (usuario.Rol != null && usuario.Rol.Nombre.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
                    return true;

                // Validar si la ruta está asociada a su rol
                return usuario.Rol?.RolPermisos.Any(rp =>
                    rp.Permiso != null &&
                    rp.Permiso.Estado &&
                    NormalizeRoute(rp.Permiso.Ruta) == route) ?? false;
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(PermisoRepository)}.{nameof(HasPermissionAsync)}", ex);
                return false;
            }
        }

        #endregion

        #region === Métodos Auxiliares ===

        private static string NormalizeRoute(string? route)
        {
            if (string.IsNullOrWhiteSpace(route))
                return string.Empty;

            route = route.ToLowerInvariant().Trim();

            // Quitar querystring
            var qIndex = route.IndexOf('?');
            if (qIndex > 0)
                route = route[..qIndex];

            // Quitar slash final
            if (route.EndsWith("/"))
                route = route[..^1];

            return route;
        }

        #endregion
    }
}
