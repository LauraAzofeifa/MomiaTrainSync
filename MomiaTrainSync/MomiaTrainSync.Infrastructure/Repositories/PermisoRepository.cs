using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Core.Interfaces.Repositories;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Infrastructure.Repositories
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

                // 👑 El Administrador tiene acceso a todo
                if (usuario.Rol!.Nombre.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
                    return true;

                // 🔐 Validar si la ruta está asociada a su rol
                return usuario.Rol.RolPermisos.Any(rp =>
                    rp.Permiso!.Estado &&
                    NormalizeRoute(rp.Permiso.Ruta) == route);
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(PermisoRepository)}.{nameof(HasPermissionAsync)}", ex);
                return false;
            }
        }

        // === Helper interno ===
        private static string NormalizeRoute(string? route)
        {
            if (string.IsNullOrWhiteSpace(route))
                return string.Empty;

            route = route.ToLowerInvariant().Trim();

            // Quitar querystring y slash final
            var qIndex = route.IndexOf('?');
            if (qIndex > 0)
                route = route[..qIndex];

            if (route.EndsWith("/"))
                route = route[..^1];

            return route;
        }
    }
}
