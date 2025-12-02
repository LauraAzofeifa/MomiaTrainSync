using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Helpers;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Core.UseCases.Base;
using MomiaTrainSync.Core.UseCases.RolesPermisos.Rol;
using System;
using System.Collections.Generic;
using System.Text;

namespace MomiaTrainSync.Core.UseCases.RolesPermisos.Permiso
{
    public class UpdatePermisoUseCase : BaseUseCase
    {
        private readonly IPermisoRepository _permisoRepository;

        public UpdatePermisoUseCase(
            IPermisoRepository permisoRepository,
            ILogErrorRepository logErrorRepository,
            IMapper mapper
        ) : base(mapper, logErrorRepository)
        {
            _permisoRepository = permisoRepository;
        }

        // ============================================================
        // 🔹 Actualizar información de permiso
        // ============================================================
        public async Task<Response<PermisoDto>> ExecuteAsync(PermisoDto dto)
        {
            return await HandleAsync(
                async () =>
                {
                    // Validación
                    var missing = ValidationHelper.ValidationRequired(
                        ("Código", dto.Codigo),
                        ("Descripción", dto.Descripcion),
                        ("Categoría", dto.Categoria),
                        ("Ruta", dto.Ruta)
                    );

                    if (missing.Any())
                        return Response<PermisoDto>.Fail(
                            $"Los siguientes campos son obligatorios: {string.Join(", ", missing)}."
                        );

                    var existing = await _permisoRepository.GetByIdAsync(dto.IdPermiso);

                    if (existing == null)
                        return Response<PermisoDto>.Fail("Permiso no encontrado.");

                    // Actualizar campos
                    existing.Codigo = dto.Codigo;
                    existing.Descripcion = dto.Descripcion;
                    existing.Categoria = dto.Categoria;
                    existing.Ruta = dto.Ruta;

                    var updated = await _permisoRepository.UpdateAsync(existing);

                    if (updated == null)
                        return Response<PermisoDto>.Fail("No se pudo actualizar el permiso.");

                    var resultDto = _mapper!.Map<PermisoDto>(updated);

                    return Response<PermisoDto>.Success(resultDto, "Permiso actualizado correctamente.");
                }
            );
        }

        // ============================================================
        // 🔹 Alternar Estado del permiso (Activar/Desactivar)
        // ============================================================
        public async Task<Response<PermisoDto>> StatusExecuteAsync(int idPermiso)
        {
            return await HandleAsync(
                async () =>
                {
                    // Validar existencia
                    var existing = await _permisoRepository.GetByIdAsync(idPermiso);

                    if (existing == null)
                        return Response<PermisoDto>.Fail("Permiso no encontrado.");

                    // Ejecutar el toggle desde el repositorio
                    var toggled = await _permisoRepository.ToggleEstadoAsync(idPermiso);

                    if (!toggled)
                        return Response<PermisoDto>.Fail("No se pudo actualizar el estado del permiso.");

                    // Obtener nuevamente la entidad para reflejar el nuevo estado
                    var updated = await _permisoRepository.GetByIdAsync(idPermiso);
                    var dto = _mapper!.Map<PermisoDto>(updated);

                    var mensaje = updated!.Estado
                        ? "Permiso activado correctamente."
                        : "Permiso desactivado correctamente.";

                    return Response<PermisoDto>.Success(dto, mensaje);
                }
            );
        }
    }
}
