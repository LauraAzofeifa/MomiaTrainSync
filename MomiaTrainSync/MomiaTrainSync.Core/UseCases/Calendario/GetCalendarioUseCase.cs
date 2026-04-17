using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.Calendario;
using MomiaTrainSync.Core.Interfaces.Repositories.Calendario;
using MomiaTrainSync.Core.Interfaces.Repositories.EntrenadorAtleta;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.UseCases.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.UseCases.Calendario
{
    public class GetCalendarioUseCase
        : BaseUseCase
    {
        private readonly ICalendarioRepository _repository;
        private readonly IEntrenadorAtletaRepository _relacionRepository;

        public GetCalendarioUseCase(
            ICalendarioRepository repository,
            IEntrenadorAtletaRepository relacionRepository,
            ILogErrorRepository logger,
            IMapper mapper
        ) : base(mapper, logger)
        {
            _repository = repository;
            _relacionRepository = relacionRepository;
        }

        public async Task<Response<IEnumerable<EntrenamientoCalendarioDto>>> ExecuteAsync(
            int idUsuario,
            bool esEntrenador,
            int cantidad = 0,
            bool incluirInactivos = false
        )
        {
            return await HandleAsync(async () =>
            {
                // Obtener el calendario directamente
                var entrenamientos = await _repository.GetAllCalendar(
                    idUsuario,
                    esEntrenador,
                    cantidad,
                    incluirInactivos
                );

                if (entrenamientos == null || !entrenamientos.Any())
                {
                    return Response<IEnumerable<EntrenamientoCalendarioDto>>
                        .Fail("No se encontraron entrenamientos para el calendario.");
                }

                return Response<IEnumerable<EntrenamientoCalendarioDto>>.Success(entrenamientos);
            });
        }
    }
}
