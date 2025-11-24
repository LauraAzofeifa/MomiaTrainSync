using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Core.Interfaces.Repositories.EntrenadorAtleta;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Domain.Entities.EntrenadorAtleta;
using MomiaTrainSync.Infrastructure.Persistence;
using MomiaTrainSync.Infrastructure.Repositories.Base;
using System;

namespace MomiaTrainSync.Infrastructure.Repositories.EntrenadorAtleta
{
    public class EntrenadorAtletaRepository : GenericRepository<EntrenadorAtletaEnt>, IEntrenadorAtletaRepository
    {

        public EntrenadorAtletaRepository(
            MomiaTrainSyncDbContext context,
            ILogErrorRepository loggerError)
        : base(context, loggerError)
        {
        }

        public async Task<List<EntrenadorAtletaEnt>> GetByEntrenadorAsync(
        int entrenadorId, bool incluirInactivos = false)
        {
            try
            {
                return await GetAllAsync(
                    include: q => q
                        .Where(ea => ea.IdEntrenador == entrenadorId)
                        .Include(ea => ea.Atleta)
                            .ThenInclude(a => a!.Rol),
                    includeInactive: incluirInactivos
                );
            }
            catch (Exception ex)
            {
                await _logger.AddLogAsync($"{nameof(EntrenadorAtletaRepository)}.{nameof(GetByEntrenadorAsync)}", ex);
                return new List<EntrenadorAtletaEnt>();
            }
        }

        public async Task<List<EntrenadorAtletaEnt>> GetByAtletaAsync(
            int atletaId, bool incluirInactivos = false)
        {
            try
            {
                return await GetAllAsync(
                    include: q => q
                        .Where(ea => ea.IdAtleta == atletaId)
                        .Include(ea => ea.Entrenador)
                            .ThenInclude(e => e!.Rol),
                    includeInactive: incluirInactivos
                );
            }
            catch (Exception ex)
            {
                await _logger.AddLogAsync($"{nameof(EntrenadorAtletaRepository)}.{nameof(GetByAtletaAsync)}", ex);
                return new List<EntrenadorAtletaEnt>();
            }
        }
    }
}
