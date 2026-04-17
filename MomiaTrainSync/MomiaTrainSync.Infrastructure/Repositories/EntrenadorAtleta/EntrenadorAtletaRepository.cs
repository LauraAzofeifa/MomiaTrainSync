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

        public override Task<EntrenadorAtletaEnt?> GetByIdAsync(int id, Func<IQueryable<EntrenadorAtletaEnt>, IQueryable<EntrenadorAtletaEnt>>? include = null, bool asNoTracking = true)
        {
            include ??= q => q
                .Include(ea => ea.Entrenador)
                    .ThenInclude(u => u!.Rol)
                .Include(ea => ea.Atleta)
                    .ThenInclude(u => u!.Rol);
            return base.GetByIdAsync(id, include, asNoTracking);
        }

        public async Task<EntrenadorAtletaEnt?> AsignarRelacionAsync(EntrenadorAtletaEnt relacion)
        {
            // 1️⃣ Verificar si ya existe una relación activa con otro entrenador
            var relacionActiva = await _context.EntrenadorAtletas
                .FirstOrDefaultAsync(ea => ea.IdAtleta == relacion.IdAtleta && ea.Estado);

            if (relacionActiva != null)
            {
                // Si es otro entrenador → no se puede asignar
                if (relacionActiva.IdEntrenador != relacion.IdEntrenador)
                    return null;

                // Si ya está activa con el mismo entrenador → devolverla
                return relacionActiva;
            }

            // 2️⃣ Verificar si existió una relación previa entre los mismos usuarios (inactiva)
            var relacionExistente = await _context.EntrenadorAtletas
                .FirstOrDefaultAsync(ea =>
                    ea.IdEntrenador == relacion.IdEntrenador &&
                    ea.IdAtleta == relacion.IdAtleta &&
                    !ea.Estado
                );

            if (relacionExistente != null)
            {
                // Reactivar
                relacionExistente.Estado = true;
                relacionExistente.FechaAsignacion = DateTime.UtcNow;

                _context.EntrenadorAtletas.Update(relacionExistente);
                await _context.SaveChangesAsync();

                return relacionExistente;
            }

            // 3️⃣ Crear nueva relación
            relacion.FechaAsignacion = DateTime.UtcNow;
            relacion.Estado = true;

            await _context.EntrenadorAtletas.AddAsync(relacion);
            await _context.SaveChangesAsync();

            return relacion;
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
