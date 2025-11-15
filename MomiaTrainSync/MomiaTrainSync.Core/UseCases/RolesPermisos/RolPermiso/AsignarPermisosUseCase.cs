using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using System;

namespace MomiaTrainSync.Core.UseCases.RolesPermisos.RolPermiso
{
    public class AsignarPermisosUseCase
    {
        private readonly IRolRepository _rolRepository;
        private readonly ILogErrorRepository _logErrorRepository;

        public AsignarPermisosUseCase(IRolRepository rolRepository, ILogErrorRepository logErrorRepository)
        {
            _rolRepository = rolRepository;
            _logErrorRepository = logErrorRepository;
        }

        public async Task<Response<bool>> ExecuteAsync(int idRol, IEnumerable<int> permisosIds)
        {
            try
            {
                if (idRol <= 0)
                    return Response<bool>.Fail("Id de rol inválido.");

                if (permisosIds == null || !permisosIds.Any())
                    return Response<bool>.Fail("Debe seleccionar al menos un permiso.");

                var success = await _rolRepository.AsignarPermisosAsync(idRol, permisosIds);

                if (!success)
                    return Response<bool>.Fail("No se pudieron asignar los permisos.");

                return Response<bool>.Success(true, "Permisos asignados correctamente.");
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(AsignarPermisosUseCase)}.{nameof(ExecuteAsync)}", ex);
                return Response<bool>.Fail("Ocurrió un error al asignar los permisos.");
            }
        }
    }
}
