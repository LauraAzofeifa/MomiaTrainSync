using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using MomiaTrainSync.Infrastructure.Persistence;
using MomiaTrainSync.Infrastructure.Repositories.Base;

public class PermisoRepository : GenericRepository<PermisoEnt>, IPermisoRepository
{
    #region Constructor

    public PermisoRepository(
        MomiaTrainSyncDbContext context,
        ILogErrorRepository logger
    ) : base(context, logger)
    {
    }

    #endregion


    #region Consultas básicas (GET)

    public async Task<List<PermisoEnt>> GetAllAsync(bool incluirInactivos = true)
    {
        try
        {
            var query = _dbSet.AsNoTracking();

            if (!incluirInactivos)
                query = query.Where(p => p.Estado);

            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            await _logger.AddLogAsync(
                $"{nameof(PermisoRepository)}.{nameof(GetAllAsync)}", ex);

            return new List<PermisoEnt>();
        }
    }

    public async Task<PermisoEnt?> GetByCodigoAsync(string codigo)
    {
        try
        {
            return await _dbSet.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Codigo == codigo);
        }
        catch (Exception ex)
        {
            await _logger.AddLogAsync(
                $"{nameof(PermisoRepository)}.{nameof(GetByCodigoAsync)}", ex);

            return null;
        }
    }

    public async Task<List<PermisoEnt>> GetByCategoriaAsync(string categoria)
    {
        try
        {
            return await _dbSet.AsNoTracking()
                .Where(p => p.Categoria == categoria && p.Estado)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            await _logger.AddLogAsync(
                $"{nameof(PermisoRepository)}.{nameof(GetByCategoriaAsync)}", ex);

            return new List<PermisoEnt>();
        }
    }

    public async Task<PermisoEnt?> GetByRutaAsync(string ruta)
    {
        try
        {
            return await _context.Permisos
                .AsNoTracking()
                .Where(p => p.Estado)
                .FirstOrDefaultAsync(p => p.Ruta == ruta);
        }
        catch (Exception ex)
        {
            await _logger.AddLogAsync(
                $"{nameof(PermisoRepository)}.{nameof(GetByRutaAsync)}", ex);

            return null;
        }
    }

    #endregion


    #region Validación de permisos

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
                .Where(u => u.Id == userId && u.Rol!.Estado)
                .FirstOrDefaultAsync();

            if (usuario == null)
                return false;

            // Administrador = acceso total
            if (usuario.Rol != null &&
                usuario.Rol.Nombre.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
                return true;

            return usuario.Rol?.RolPermisos.Any(rp =>
                rp.Permiso != null &&
                rp.Permiso.Estado &&
                NormalizeRoute(rp.Permiso.Ruta) == route
            ) ?? false;
        }
        catch (Exception ex)
        {
            await _logger.AddLogAsync(
                $"{nameof(PermisoRepository)}.{nameof(HasPermissionAsync)}", ex);

            return false;
        }
    }

    #endregion


    #region Helpers

    private static string NormalizeRoute(string? route)
    {
        if (string.IsNullOrWhiteSpace(route))
            return string.Empty;

        route = route.ToLowerInvariant().Trim();

        // eliminar query params
        var qIndex = route.IndexOf('?');
        if (qIndex > 0)
            route = route[..qIndex];

        // eliminar "/" final
        if (route.EndsWith("/"))
            route = route[..^1];

        return route;
    }

    #endregion
}
