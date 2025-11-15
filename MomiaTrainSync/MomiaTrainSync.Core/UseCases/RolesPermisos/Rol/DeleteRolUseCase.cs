using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.UseCases.RolesPermisos.Rol
{
    public class DeleteRolUseCase
    {
        private readonly IRolRepository _rolRepository;
        private readonly ILogErrorRepository _logErrorRepository;

        public DeleteRolUseCase(IRolRepository rolRepository, ILogErrorRepository logErrorRepository)
        {
            _rolRepository = rolRepository;
            _logErrorRepository = logErrorRepository;
        }

        public async Task<Response<bool>> ExecuteAsync(int idRol)
        {
            try
            {
                var deleted = await _rolRepository.DeleteAsync(idRol);
                if (!deleted)
                    return Response<bool>.Fail("No se pudo eliminar el rol.");

                return Response<bool>.Success(true, "Rol eliminado correctamente.");
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(DeleteRolUseCase)}.{nameof(ExecuteAsync)}", ex);
                return Response<bool>.Fail("Ocurrió un error al eliminar el rol.");
            }
        }
    }
}
