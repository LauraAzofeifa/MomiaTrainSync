using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.Interfaces.Repositories.EntrenadorAtleta;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using System;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.UseCases.TrainerAthleteUseCase
{
    public class DeleteAthleteUseCase
    {
        private readonly IEntrenadorAtletaRepository _entrenadorAtletaRepository;
        private readonly ILogErrorRepository _logErrorRepository;

        public DeleteAthleteUseCase(
            IEntrenadorAtletaRepository entrenadorAtletaRepository,
            ILogErrorRepository logErrorRepository)
        {
            _entrenadorAtletaRepository = entrenadorAtletaRepository;
            _logErrorRepository = logErrorRepository;
        }

        public async Task<Response<bool>> ExecuteAsync(int idRelacion)
        {
            try
            {
                // Obtener la relación para validar su existencia
                var relacion = await _entrenadorAtletaRepository.GetByIdAsync(idRelacion);
                if (relacion == null)
                    return Response<bool>.Fail("La relación entrenador-atleta no existe.");

                if (!relacion.Estado)
                    return Response<bool>.Fail("La relación ya se encuentra desactivada.");

                // Realizar la eliminación lógica
                await _entrenadorAtletaRepository.DeleteAsync(idRelacion);

                return Response<bool>.Success(true, "El atleta fue desvinculado exitosamente del entrenador.");
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync(
                    $"{nameof(DeleteAthleteUseCase)}.{nameof(ExecuteAsync)}", ex
                );

                return Response<bool>.Fail("Ocurrió un error inesperado al eliminar la relación entrenador-atleta.");
            }
        }
    }
}