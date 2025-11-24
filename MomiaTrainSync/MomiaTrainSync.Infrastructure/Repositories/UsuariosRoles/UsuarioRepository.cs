using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using MomiaTrainSync.Infrastructure.Persistence;
using MomiaTrainSync.Infrastructure.Repositories.Base;

namespace MomiaTrainSync.Infrastructure.Repositories.UsuariosRoles
{
    public class UsuarioRepository
        : GenericRepository<UsuarioEnt>, IUsuarioRepository
    {
        public UsuarioRepository(
            MomiaTrainSyncDbContext context,
            ILogErrorRepository logErrorRepository)
            : base(context, logErrorRepository)
        {
        }

        public override Task<List<UsuarioEnt>> GetAllAsync(bool asNoTracking = true, Func<IQueryable<UsuarioEnt>, IQueryable<UsuarioEnt>>? include = null, bool includeInactive = false)
        {
            return base.GetAllAsync(
                asNoTracking,
                q => q.Include(u => u.Rol),
                includeInactive
             );
        }

        // ============================================================
        // MÉTODOS PERSONALIZADOS DEL REPOSITORIO
        // ============================================================

        public async Task<UsuarioEnt?> GetByEmailAsync(string email)
        {
            return await FirstAsync(
                u => u.Correo == email,
                include: q => q.Include(u => u.Rol)
            );
        }

        public async Task<UsuarioEnt?> GetByIdWithRolAsync(int id)
        {
            return await FirstAsync(
                u => u.Id == id,
                include: q => q.Include(u => u.Rol)
            );
        }

        public async Task<List<UsuarioEnt>> GetAtletasByEntrenadorAsync(int entrenadorId, bool includeInactive = false)
        {
            try
            {
                var query = _context.EntrenadorAtletas
                    .Where(ea => ea.IdEntrenador == entrenadorId)
                    .Include(ea => ea.Atleta)
                        .ThenInclude(a => a!.Rol)
                    .Select(ea => ea.Atleta)
                    .AsQueryable();

                if (!includeInactive)
                    query = query.Where(a => a != null && a.Estado);

                return await query
                    .Where(a => a != null)
                    .Select(a => a!)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await _logger.AddLogAsync(
                    $"{nameof(UsuarioRepository)}.{nameof(GetAtletasByEntrenadorAsync)}",
                    ex);
                return new List<UsuarioEnt>();
            }
        }
    }
}
