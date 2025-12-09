using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.Home;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.RutinasEntrenamientos;
using MomiaTrainSync.Core.UseCases.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.UseCases.Home
{
    public class GetHomeUseCase : BaseUseCase
    {
        private readonly IRutinaRepository _rutinaRepository;

        public GetHomeUseCase(
            IRutinaRepository rutinaRepository,
            IMapper? mapper, 
            ILogErrorRepository logger
            ) : base(mapper, logger)
        {
            _rutinaRepository = rutinaRepository;
        }

        public async Task<Response<HomeDto>> ExecuteAsync(int IdUsuario)
        {
            return await HandleAsync(async () =>
            {
                var rutinas = await _rutinaRepository.ContarRutinasActivasAsync(IdUsuario);

                var homeDto = new HomeDto
                {
                    RutinasActivas = rutinas,
                };

                return Response<HomeDto>.Success(homeDto);
            });
        }
    }
}
