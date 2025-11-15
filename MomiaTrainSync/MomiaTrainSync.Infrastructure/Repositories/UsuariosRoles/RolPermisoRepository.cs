using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Domain.Entities;
using MomiaTrainSync.Infrastructure.Persistence;

namespace MomiaTrainSync.Infrastructure.Repositories.UsuariosRoles
{
    public class RolPermisoRepository : IRolPermisoRepository
    {
        private readonly MomiaTrainSyncDbContext _context;
        private readonly ILogErrorRepository _logErrorRepository;

        public RolPermisoRepository(MomiaTrainSyncDbContext context, ILogErrorRepository logErrorRepository)
        {
            _context = context;
            _logErrorRepository = logErrorRepository;
        }

        public async Task<bool> ExistsAsync(int idRol, int idPermiso)
        {
            try
            {
                return await _context.RolesPermisos.AnyAsync(rp => rp.IdRol == idRol && rp.IdPermiso == idPermiso);
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(RolPermisoRepository)}.{nameof(ExistsAsync)}", ex);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int idRol, int idPermiso)
        {
            try
            {
                var rel = await _context.RolesPermisos.FirstOrDefaultAsync(rp => rp.IdRol == idRol && rp.IdPermiso == idPermiso);
                if (rel == null)
                    return false;

                _context.RolesPermisos.Remove(rel);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(RolPermisoRepository)}.{nameof(DeleteAsync)}", ex);
                return false;
            }
        }
    }
}
