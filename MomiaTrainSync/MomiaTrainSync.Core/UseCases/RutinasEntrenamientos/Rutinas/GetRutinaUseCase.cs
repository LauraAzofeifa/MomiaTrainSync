using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.RutinasEntrenamientos;
using MomiaTrainSync.Core.Interfaces.Repositories.EntrenadorAtleta;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.RutinasEntrenamientos;
using MomiaTrainSync.Core.UseCases.Base;
using MomiaTrainSync.Domain.Entities.RutinasAsignaciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.UseCases.RutinasEntrenamientos.Rutinas
{
    public class GetRutinaUseCase : BaseUseCase
    {
        private readonly IRutinaRepository _repository;
        private readonly IEntrenadorAtletaRepository _entrenadorAtletaRepository;

        public GetRutinaUseCase(
            IRutinaRepository repository,
            IEntrenadorAtletaRepository entrenadorAtletaRepository,
            ILogErrorRepository logError,
            IMapper mapper
            ) : base (mapper, logError)
        {
            _entrenadorAtletaRepository = entrenadorAtletaRepository;
            _repository = repository;
        }

        public Task<Response<IEnumerable<RutinaDto>>> ExecuteAsync(
            int? idRutina = null,
            int? idRelacion = null,
            bool incluirInactivos = false
            )
        {
            return HandleAsync(async () =>
            {
                // Validamos que el usuario esté activo (entrenador y/o atleta).
                if (idRelacion.HasValue)
                {
                    var relacion = await _entrenadorAtletaRepository
                        .GetByIdAsync(idRelacion.Value);

                    if (relacion == null)
                    {
                        return Response<IEnumerable<RutinaDto>>.Fail(
                            "La relación entrenador–atleta no existe."
                        );
                    }

                    bool entrenadorInactivo = relacion.Entrenador?.Estado == true;
                    bool atletaInactivo = relacion.Atleta?.Estado == true;

                    if (!incluirInactivos && (entrenadorInactivo || atletaInactivo))
                    {
                        return Response<IEnumerable<RutinaDto>>.Fail(
                            "La relación contiene usuarios inactivos y no se permite ver información inactiva."
                        );
                    }
                }


                var data = await _repository.GetRutinasAsync(idRutina, idRelacion, incluirInactivos);

                if (data is null || !data.Any())
                {
                    return Response<IEnumerable<RutinaDto>>.Fail(
                        "No se encontraron rutinas.",
                        Enumerable.Empty<RutinaDto>()
                    );
                }

                var rutinaDto = _mapper!.Map<IEnumerable<RutinaDto>>(data);

                return Response<IEnumerable<RutinaDto>>.Success(
                    rutinaDto,
                    "Rutinas obtenidas correctamente."
                );
            });
        }

    }
}
