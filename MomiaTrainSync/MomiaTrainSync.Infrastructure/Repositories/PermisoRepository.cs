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

        public async Task<bool> HasPermissionAsync(int userId, string permissionName)
        {
            try
            {
                return await _context.Usuarios
                    .Include(u => u.Rol)
                    .ThenInclude(r => r.RolPermisos)
                    .ThenInclude(rp => rp.Permiso)
                    .AnyAsync(u =>
                        u.Id == userId &&
                        u.Rol.RolPermisos.Any(rp => rp.Permiso.Codigo == permissionName));
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(PermisoRepository)}.{nameof(HasPermissionAsync)}", ex);
                return false;
            }
        }
    }
}
