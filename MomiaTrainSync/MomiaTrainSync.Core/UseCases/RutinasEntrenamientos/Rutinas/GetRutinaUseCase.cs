using AutoMapper;
using MomiaTrainSync.Core.Common;
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

        public GetRutinaUseCase(
            IRutinaRepository repository,
            ILogErrorRepository logError,
            IMapper mapper
            ) : base (mapper, logError)
        {
            _repository = repository;
        }

        public Task<Response<IEnumerable<RutinaEnt>>> ExecuteAsync(
            int? idRutina = null,
            int? idRelacion = null,
            bool incluirInactivos = false
            )
        {
            return HandleAsync(async () =>
            {
                var data = await _repository.GetRutinasAsync(idRutina, idRelacion, incluirInactivos);

                if ( data is null || !data.Any())
                {
                    return Response<IEnumerable<RutinaEnt>>.Fail(
                        "No se encontraron rutinas."
                     );
                }

                return Response<IEnumerable<RutinaEnt>>.Success(
                    data,
                    "Rutinas obtenidas correctamente."
                );
            });
        }
    }
}
