using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.UseCases.Base
{
    public abstract class BaseUseCase
    {
        // El mapper puede ser null si el caso de uso no necesita mapeo.
        protected readonly IMapper? _mapper;
        protected readonly ILogErrorRepository _logger;

        // Constructor principal: acepta mapper opcional.
        protected BaseUseCase(IMapper? mapper, ILogErrorRepository logger)
        {
            _mapper = mapper;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Constructor auxiliar para escenarios donde no hay IMapper disponible/inyectado.
        protected BaseUseCase(ILogErrorRepository logger)
            : this(null, logger)
        {
        }

        protected async Task<Response<T>> HandleAsync<T>(Func<Task<Response<T>>> action, string? messageFail = null)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                await _logger.AddLogAsync(
                    $"{GetType().Name}.{nameof(HandleAsync)}",
                    ex
                );

                return Response<T>.Fail(messageFail ?? "Ocurrió un error inesperado.");
            }
        }
    }

}
