using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Core.Interfaces.Repositories.EntrenadorAtleta;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Domain.Entities.EntrenadorAtleta;
using MomiaTrainSync.Infrastructure.Persistence;
using System;

namespace MomiaTrainSync.Infrastructure.Repositories.EntrenadorAtleta
{
    public class EntrenadorAtletaRepository : IEntrenadorAtletaRepository
    {
        private readonly MomiaTrainSyncDbContext _context;
        private readonly ILogErrorRepository _logErrorRepository;

        public EntrenadorAtletaRepository(MomiaTrainSyncDbContext context, ILogErrorRepository logErrorRepository)
        {
            _context = context;
            _logErrorRepository = logErrorRepository;
        }

        public async Task<EntrenadorAtletaEnt?> GetByIdAsync(int id)
        {
            return await _context.EntrenadorAtletas
                .Include(ea => ea.Entrenador)
                .Include(ea => ea.Atleta)
                .FirstOrDefaultAsync(ea => ea.IdRelacion == id);
        }

        public async Task<List<EntrenadorAtletaEnt>> GetAllAsync(bool incluirInactivos = false)
        {
            try
            {
                var query = _context.EntrenadorAtletas
                    .Include(ea => ea.Entrenador)
                    .Include(ea => ea.Atleta)
                    .AsNoTracking();

                if (!incluirInactivos)
                    query = query.Where(ea => ea.Estado);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(EntrenadorAtletaRepository)}.{nameof(GetAllAsync)}", ex);
                return new List<EntrenadorAtletaEnt>();
            }
        }

        public async Task<List<EntrenadorAtletaEnt>> GetByEntrenadorAsync(int entrenadorId, bool incluirInactivos = false)
        {
            try
            {
                var query = _context.EntrenadorAtletas
                    .Where(ea => ea.IdEntrenador == entrenadorId)
                    .Include(ea => ea.Atleta)
                        .ThenInclude(a => a!.Rol)
                    .AsNoTracking();

                if (!incluirInactivos)
                    query = query.Where(ea => ea.Estado && ea.Atleta != null && ea.Atleta.Estado);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(EntrenadorAtletaRepository)}.{nameof(GetByEntrenadorAsync)}", ex);
                return new List<EntrenadorAtletaEnt>();
            }
        }

        public async Task<List<EntrenadorAtletaEnt>> GetByAtletaAsync(int atletaId, bool incluirInactivos = false)
        {
            try
            {
                var query = _context.EntrenadorAtletas
                    .Where(ea => ea.IdAtleta == atletaId)
                    .Include(ea => ea.Entrenador)
                        .ThenInclude(e => e!.Rol)
                    .AsNoTracking();

                if (!incluirInactivos)
                    query = query.Where(ea => ea.Estado && ea.Entrenador != null && ea.Entrenador.Estado);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(EntrenadorAtletaRepository)}.{nameof(GetByAtletaAsync)}", ex);
                return new List<EntrenadorAtletaEnt>();
            }
        }

        public async Task<EntrenadorAtletaEnt?> AddAsync(EntrenadorAtletaEnt relacion)
        {
            try
            {
                // Buscar relaciones activas de este atleta con otro entrenador
                var relacionActiva = await _context.EntrenadorAtletas
                    .FirstOrDefaultAsync(ea => ea.IdAtleta == relacion.IdAtleta && ea.Estado);

                if (relacionActiva != null)
                {
                    // Si ya tiene un entrenador activo distinto → no se puede asignar
                    if (relacionActiva.IdEntrenador != relacion.IdEntrenador)
                        return null;

                    // Si es el mismo entrenador → ya está activa, no hacer nada
                    return relacionActiva;
                }

                // Buscar si ya existió una relación entre este entrenador y atleta (inactiva)
                var relacionExistente = await _context.EntrenadorAtletas
                    .FirstOrDefaultAsync(ea => ea.IdEntrenador == relacion.IdEntrenador &&
                                               ea.IdAtleta == relacion.IdAtleta &&
                                               !ea.Estado);

                if (relacionExistente != null)
                {
                    // Reactivar la relación inactiva
                    relacionExistente.Estado = true;
                    relacionExistente.FechaAsignacion = DateTime.UtcNow;
                    _context.EntrenadorAtletas.Update(relacionExistente);
                    await _context.SaveChangesAsync();

                    return relacionExistente;
                }

                // Si no existe ninguna relación previa → crear una nueva
                relacion.FechaAsignacion = DateTime.UtcNow;
                relacion.Estado = true;

                await _context.EntrenadorAtletas.AddAsync(relacion);
                await _context.SaveChangesAsync();

                return relacion;
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(EntrenadorAtletaRepository)}.{nameof(AddAsync)}", ex);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var relacion = await _context.EntrenadorAtletas.FindAsync(id);
                if (relacion == null)
                    return;

                relacion.Estado = false;
                _context.EntrenadorAtletas.Update(relacion);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(EntrenadorAtletaRepository)}.{nameof(DeleteAsync)}", ex);
                throw;
            }
        }
    }
}
