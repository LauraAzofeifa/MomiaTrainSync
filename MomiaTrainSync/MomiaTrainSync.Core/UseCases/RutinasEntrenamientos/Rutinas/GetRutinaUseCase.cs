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
                        return Response<IEnumerable<RutinaDto>>.Fail("La relación entrenador–atleta no existe.");
                    }

                    // Estado == true -> activo. Estado == false -> inactivo (o eliminado lógico)
                    bool entrenadorInactivo = relacion.Entrenador != null && relacion.Entrenador.Estado == false;
                    bool atletaInactivo = relacion.Atleta != null && relacion.Atleta.Estado == false;

                    if (incluirInactivos && (entrenadorInactivo || atletaInactivo))
                    {
                        var partes = new List<string>();
                        if (entrenadorInactivo) partes.Add("entrenador");
                        if (atletaInactivo) partes.Add("atleta");

                        return Response<IEnumerable<RutinaDto>>.Fail(
                            $"No se permiten resultados: el/los usuario(s) {string.Join(" y ", partes)} están inactivo(s)."
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
