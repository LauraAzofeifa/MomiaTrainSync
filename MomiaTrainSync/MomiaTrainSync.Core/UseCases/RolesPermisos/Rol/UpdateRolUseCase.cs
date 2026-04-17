using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Helpers;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Core.UseCases.Base;
using System;

namespace MomiaTrainSync.Core.UseCases.RolesPermisos.Rol
{
    public class UpdateRolUseCase : BaseUseCase
    {
        private readonly IRolRepository _rolRepository;

        public UpdateRolUseCase(
            IRolRepository rolRepository,
            ILogErrorRepository logErrorRepository,
            IMapper mapper
        )
        : base(mapper, logErrorRepository)
        {
            _rolRepository = rolRepository;
        }

        // ----------------------------------------------------------
        // 🔹 Actualizar Datos del Rol
        // ----------------------------------------------------------
        public async Task<Response<RolDto>> ExecuteAsync(RolDto dto)
        {
            return await HandleAsync(
                async () =>
                {
                    var existing = await _rolRepository.GetByIdAsync(dto.IdRol);
                    if (existing == null)
                        return Response<RolDto>.Fail("Rol no encontrado.");

                    // Validación con helper
                    var missing = ValidationHelper.ValidationRequired(
                        ("Nombre del Rol", dto.Nombre)
                    );

                    if (missing.Any())
                        return Response<RolDto>.Fail(
                            $"Los siguientes campos son obligatorios: {string.Join(", ", missing)}."
                        );

                    // Mapear manualmente (evita sobrescribir cosas no permitidas)
                    existing.Nombre = dto.Nombre;
                    existing.Descripcion = dto.Descripcion;

                    var updated = await _rolRepository.UpdateAsync(existing);

                    if (updated == null)
                        return Response<RolDto>.Fail("No se pudo actualizar el rol.");

                    var result = _mapper!.Map<RolDto>(existing);

                    return Response<RolDto>.Success(result, "Rol actualizado correctamente.");
                }
            );
        }

        // ----------------------------------------------------------
        // 🔹 Alternar Estado (Activo ↔ Inactivo)
        // ----------------------------------------------------------
        public async Task<Response<bool>> StatusExecuteAsync(int idRol)
        {
            return await HandleAsync(
                async () =>
                {
                    var existing = await _rolRepository.GetByIdAsync(idRol);
                    if (existing == null)
                        return Response<bool>.Fail("Rol no encontrado.");

                    var updated = await _rolRepository.ToggleEstadoAsync(idRol);
                    if (!updated)
                        return Response<bool>.Fail("No se pudo actualizar el estado del rol.");

                    var refreshed = await _rolRepository.GetByIdAsync(idRol);

                    if (refreshed == null)
                        return Response<bool>.Fail("No se pudo obtener el estado actualizado.");

                    var mensaje = refreshed.Estado
                        ? "Rol activado correctamente."
                        : "Rol desactivado correctamente.";

                    return Response<bool>.Success(true, mensaje);
                }
            );
        }
    }
}
