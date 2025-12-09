using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.RutinasEntrenamientos;
using MomiaTrainSync.Domain.Entities.EntrenamientosZonas;
using MomiaTrainSync.Infrastructure.Persistence;
using MomiaTrainSync.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Infrastructure.Repositories.RutinasEntrenamientos
{
    public class EntrenamientoRepository : GenericRepository<EntrenamientoEnt>, IEntrenamientoRepository
    {
        public EntrenamientoRepository(
            MomiaTrainSyncDbContext context,
            ILogErrorRepository loggerError
            ) : base( context, loggerError )
        {
            
        }

        public async Task<List<EntrenamientoEnt>> GetByRutinaAsync(int idRutina)
        {
            return await GetAllAsync(
                include: q => q.Where(rt => rt.IdRutina == idRutina)    
            );
        }

        public async Task<List<EntrenamientoEnt>> GetEntrenamientosAsync(
            int? IdEntrenamiento = null,
            int? IdRutina = null,
            bool incluirInactivos = false
            )
        {
            return await GetAllAsync(
                include: q =>
                {
                    q = q.Include(e => e.TipoSesion);

                    if (IdEntrenamiento.HasValue)
                        q = q.Where(e => e.IdEntrenamiento == IdEntrenamiento.Value);
                    if (IdRutina.HasValue)
                        q = q.Where(e => e.IdRutina == IdRutina.Value);
                    if (incluirInactivos)
                        q = q.Where(e => e.Estado);
                    return q;
                }
            );
        }

        public async Task<bool> ToggleEstadoByRutinaIdAsync(int idRutina, bool nuevoEstado)
        {
            try
            {
                var entrenamientos = await _dbSet
                    .Where(e => e.IdRutina == idRutina)
                    .ToListAsync();

                if (!entrenamientos.Any())
                    return true;

                foreach (var ent in entrenamientos)
                {
                    ent.Estado = nuevoEstado;
                    _dbSet.Update(ent);
                }

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                await Log(nameof(ToggleEstadoByRutinaIdAsync), ex);
                return false;
            }
        } 
    }
}
