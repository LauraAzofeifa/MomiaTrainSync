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
        protected readonly IMapper _mapper;
        protected readonly ILogErrorRepository _logger;

        protected BaseUseCase(
            IMapper mapper,
            ILogErrorRepository logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        protected async Task<Response<T>> HandleAsync<T>(Func<Task<Response<T>>> action)
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

                return Response<T>.Fail("Ocurrió un error inesperado.");
            }
        }
    }

}
