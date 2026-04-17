using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Helpers;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Core.UseCases.Base;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.UseCases.RolesPermisos.Permiso
{
    public class AddPermisoUseCase : BaseUseCase
    {
        private readonly IPermisoRepository _permisoRepository;

        public AddPermisoUseCase(
            IPermisoRepository permisoRepository,
            ILogErrorRepository logErrorRepository,
            IMapper mapper
        ) : base(mapper, logErrorRepository)
        {
            _permisoRepository = permisoRepository;
        }

        public async Task<Response<PermisoDto>> ExecuteAsync(PermisoDto dto)
        {
            return await HandleAsync(
                async () =>
                {
                    // 🔍 Validación usando ValidationHelper
                    var missingFields = ValidationHelper.ValidationRequired(
                        ("Código", dto.Codigo),
                        ("Descripción", dto.Descripcion),
                        ("Categoría", dto.Categoria),
                        ("Ruta", dto.Ruta)
                    );

                    if (missingFields.Any())
                    {
                        var fields = string.Join(", ", missingFields);
                        return Response<PermisoDto>.Fail($"Los siguientes campos son obligatorios: {fields}.");
                    }

                    //Ponemos el permiso como activo
                    dto.Estado = true;

                    // Mapear DTO → Entidad
                    var entity = _mapper!.Map<PermisoEnt>(dto);

                    // Crear permiso
                    var created = await _permisoRepository.AddAsync(entity);

                    if (created == null)
                        return Response<PermisoDto>.Fail("No se pudo crear el permiso.");

                    // Mapear Entidad → DTO
                    var resultDto = _mapper.Map<PermisoDto>(created);

                    return Response<PermisoDto>.Success(resultDto, "Permiso creado correctamente.");
                }
            );
        }
    }
}
