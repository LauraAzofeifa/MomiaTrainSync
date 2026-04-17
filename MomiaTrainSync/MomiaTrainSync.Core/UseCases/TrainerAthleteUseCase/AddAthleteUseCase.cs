using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.EntrenadorAtleta;
using MomiaTrainSync.Core.Interfaces.Repositories.EntrenadorAtleta;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Domain.Entities.EntrenadorAtleta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.UseCases.TrainerAthleteUseCase
{
    public class AddAthleteUseCase
    {
        private readonly IEntrenadorAtletaRepository _entrenadorAtletaRepository;
        private readonly ILogErrorRepository _logErrorRepository;
        private readonly IMapper _mapper;

        public AddAthleteUseCase(
            IEntrenadorAtletaRepository entrenadorAtletaRepository,
            ILogErrorRepository logErrorRepository,
            IMapper mapper)
        {
            _entrenadorAtletaRepository = entrenadorAtletaRepository;
            _logErrorRepository = logErrorRepository;
            _mapper = mapper;
        }

        public async Task<Response<bool>> ExecuteAsync(EntrenadorAtletaDto entrenadorAtletaDto)
        {
            try
            {
                var entidad = _mapper.Map<EntrenadorAtletaEnt>(entrenadorAtletaDto);

                var result = await _entrenadorAtletaRepository.AsignarRelacionAsync(entidad);

                if (result == null)
                    return Response<bool>.Fail("El atleta ya tiene un entrenador activo.");

                // Si llegó aquí, fue éxito: crear o reactivar
                var mensaje = result.IdRelacion > 0 && result.FechaAsignacion.Date == DateTime.UtcNow.Date
                    ? "El atleta fue asignado correctamente al entrenador."
                    : "La relación entrenador-atleta fue reactivada exitosamente.";

                return Response<bool>.Success(true, mensaje);
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync(
                    $"{nameof(AddAthleteUseCase)}.{nameof(ExecuteAsync)}", ex
                );

                return Response<bool>.Fail("Ocurrió un error inesperado al asignar el atleta.");
            }
        }

    }
}
