using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Core.DTOs.Calendario;
using MomiaTrainSync.Core.Interfaces.Repositories.Calendario;
using MomiaTrainSync.Core.Interfaces.Repositories.EntrenadorAtleta;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Domain.Entities.EntrenamientosZonas;
using MomiaTrainSync.Infrastructure.Persistence;
using MomiaTrainSync.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Infrastructure.Repositories.Calendario
{
    public class CalendarioRepository : GenericRepository<EntrenamientoEnt>, ICalendarioRepository
    {
        private readonly IEntrenadorAtletaRepository _relacionRepository;

        public CalendarioRepository(
            IEntrenadorAtletaRepository relacionRepository,
            MomiaTrainSyncDbContext context, 
            ILogErrorRepository logger
            ) : base(context, logger)
        {
            _relacionRepository = relacionRepository;
        }

        public async Task<List<EntrenamientoCalendarioDto>> GetAllCalendar(
            int idUsuario,
            bool esEntrenador,
            int cantidad = 0,
            bool incluirInactivos = false
        )
        {
            IEnumerable<int> relacionesIds;

            if (esEntrenador)
            {
                // Obtener TODAS las relaciones donde él es entrenador
                var relaciones = await _relacionRepository.GetByEntrenadorAsync(idUsuario, incluirInactivos);

                // Solo relaciones activas
                relacionesIds = relaciones
                    .Where(r => r.Estado)
                    .Select(r => r.IdRelacion);
            }
            else
            {
                // Obtener TODAS las relaciones del atleta
                var relaciones = await _relacionRepository.GetByAtletaAsync(idUsuario, incluirInactivos);

                // Solo 1 activa (máximo una)
                var relacionActiva = relaciones.FirstOrDefault(r => r.Estado);

                relacionesIds = relacionActiva != null
                    ? new List<int> { relacionActiva.IdRelacion }
                    : new List<int>();
            }

            // Si no tiene relaciones activas, devolvemos vacío
            if (!relacionesIds.Any())
                return new List<EntrenamientoCalendarioDto>();

            // 2. Cargar entrenamientos
            var entrenamientos = await GetAllAsync(
                include: q =>
                    q.Include(e => e.TipoSesion)
                     .Include(e => e.Rutina)
                        .ThenInclude(r => r.Relacion)
                            .ThenInclude(rel => rel.Entrenador)
                     .Include(e => e.Rutina)
                        .ThenInclude(r => r.Relacion)
                            .ThenInclude(rel => rel.Atleta)
                     .Where(e => relacionesIds.Contains(e.Rutina!.IdRelacion))
                     .Where(e => incluirInactivos || (e.Estado && e.Rutina!.Estado))
                     .OrderBy(e => e.FechaProgramada),
                includeInactive: incluirInactivos
            );

            if (cantidad > 0)
                entrenamientos = entrenamientos.Take(cantidad).ToList();

            // 3. Mapear DTO
            return entrenamientos.Select(e => new EntrenamientoCalendarioDto
            {
                IdEntrenamiento = e.IdEntrenamiento,
                NombreEntrenamiento = e.Nombre,
                ObjetivoEntrenamiento = e.Objetivo!,
                DuracionEstimada = e.DuracionEstimada,
                TipoSesionNombre = e.TipoSesion!.Nombre,
                FechaProgramada = e.FechaProgramada,

                IdRutina = e.Rutina!.IdRutina,
                NombreRutina = e.Rutina.Nombre,
                DescripcionRutina = e.Rutina.Descripcion,

                IdEntrenador = e.Rutina.Relacion!.IdEntrenador,
                NombreEntrenador = e.Rutina.Relacion.Entrenador!.Nombre,

                IdAtleta = e.Rutina.Relacion!.IdAtleta,
                NombreAtleta = e.Rutina.Relacion.Atleta!.Nombre
            })
            .ToList();
        }
    }
}
