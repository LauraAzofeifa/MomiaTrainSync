using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using MomiaTrainSync.Infrastructure.Persistence;
using MomiaTrainSync.Infrastructure.Repositories.Base;

public class RolPermisoRepository
    : GenericRepository<RolPermisoEnt>, IRolPermisoRepository
{
    public RolPermisoRepository(
        MomiaTrainSyncDbContext context,
        ILogErrorRepository logger
    ) : base(context, logger)
    {
    }

    public async Task<bool> ExistsAsync(int idRol, int idPermiso)
    {
        try
        {
            return await _dbSet
                .AnyAsync(rp => rp.IdRol == idRol && rp.IdPermiso == idPermiso);
        }
        catch (Exception ex)
        {
            await _logger.AddLogAsync(
                $"{nameof(RolPermisoRepository)}.{nameof(ExistsAsync)}", ex);
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int idRol, int idPermiso)
    {
        try
        {
            var rel = await _dbSet
                .FirstOrDefaultAsync(rp => rp.IdRol == idRol && rp.IdPermiso == idPermiso);

            if (rel == null)
                return false;

            _dbSet.Remove(rel);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            await _logger.AddLogAsync(
                $"{nameof(RolPermisoRepository)}.{nameof(DeleteAsync)}", ex);
            return false;
        }
    }
}
