using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Core.UseCases.Base;
using System;

namespace MomiaTrainSync.Core.UseCases.RolesPermisos.RolPermiso
{
    public class AsignarPermisosUseCase : BaseUseCase
    {
        private readonly IRolRepository _rolRepository;

        public AsignarPermisosUseCase(
            IRolRepository rolRepository,
            ILogErrorRepository logError
            ) : base(logError)
        {
            _rolRepository = rolRepository;
        }

        public async Task<Response<bool>> ExecuteAsync(int idRol, IEnumerable<int> permisosIds)
        {
            return await HandleAsync(
                async () =>
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
            );
        }
    }
}
