using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.EntrenadorAtleta;
using MomiaTrainSync.Core.Interfaces.Repositories.EntrenadorAtleta;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Domain.Entities.EntrenadorAtleta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.UseCases.TrainerAthleteUseCase
{
    public class GetEntrenadorAtletaUseCase
    {
        private readonly IEntrenadorAtletaRepository _entrenadorAtletaRepository;
        private readonly ILogErrorRepository _logErrorRepository;
        private readonly IMapper _mapper;

        public GetEntrenadorAtletaUseCase(
            IEntrenadorAtletaRepository entrenadorAtletaRepository,
            ILogErrorRepository logErrorRepository,
            IMapper mapper)
        {
            _entrenadorAtletaRepository = entrenadorAtletaRepository;
            _logErrorRepository = logErrorRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<EntrenadorAtletaDto>>> ExecuteAsync(
            int? idRelacion = null,
            int? entrenadorId = null,
            int? atletaId = null,
            bool incluirInactivos = false)
        {
            try
            {
                IEnumerable<EntrenadorAtletaEnt> relaciones;

                // Obtener relación por ID
                if (idRelacion.HasValue)
                {
                    var relacion = await _entrenadorAtletaRepository.GetByIdAsync(idRelacion.Value);
                    relaciones = relacion != null
                        ? new List<EntrenadorAtletaEnt> { relacion }
                        : Enumerable.Empty<EntrenadorAtletaEnt>();
                }
                // Obtener relaciones de un entrenador
                else if (entrenadorId.HasValue)
                {
                    relaciones = await _entrenadorAtletaRepository.GetByEntrenadorAsync(entrenadorId.Value, incluirInactivos);
                }
                // Obtener relaciones de un atleta
                else if (atletaId.HasValue)
                {
                    relaciones = await _entrenadorAtletaRepository.GetByAtletaAsync(atletaId.Value, incluirInactivos);
                }
                // Obtener todas las relaciones
                else
                {
                    relaciones = await _entrenadorAtletaRepository.GetAllAsync(incluirInactivos);
                }

                var relacionesDto = _mapper.Map<IEnumerable<EntrenadorAtletaDto>>(relaciones);

                if (!relacionesDto.Any())
                    return Response<IEnumerable<EntrenadorAtletaDto>>.Fail("No se encontraron relaciones entrenador-atleta.");

                return Response<IEnumerable<EntrenadorAtletaDto>>.Success(
                    relacionesDto,
                    "Relaciones entrenador-atleta obtenidas correctamente."
                );
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync(
                    $"{nameof(GetEntrenadorAtletaUseCase)}.{nameof(ExecuteAsync)}", ex);
                return Response<IEnumerable<EntrenadorAtletaDto>>.Fail(
                    "Error al obtener las relaciones: " + ex.Message);
            }
        }
    }
}
