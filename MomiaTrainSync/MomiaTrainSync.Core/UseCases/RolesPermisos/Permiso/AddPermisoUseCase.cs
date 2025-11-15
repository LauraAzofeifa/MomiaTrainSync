using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.UseCases.RolesPermisos.Permiso
{
    public class AddPermisoUseCase
    {
        private readonly IPermisoRepository _permisoRepository;
        private readonly ILogErrorRepository _logErrorRepository;
        private readonly IMapper _mapper;

        public AddPermisoUseCase(
            IPermisoRepository permisoRepository,
            ILogErrorRepository logErrorRepository,
            IMapper mapper)
        {
            _permisoRepository = permisoRepository;
            _logErrorRepository = logErrorRepository;
            _mapper = mapper;
        }

        public async Task<Response<PermisoDto>> ExecuteAsync(PermisoDto dto)
        {
            try
            {
                // Validación clara y específica
                var missingFields = new List<string>();

                if (string.IsNullOrWhiteSpace(dto.Codigo)) missingFields.Add("Código");
                if (string.IsNullOrWhiteSpace(dto.Descripcion)) missingFields.Add("Descripción");
                if (string.IsNullOrWhiteSpace(dto.Categoria)) missingFields.Add("Categoría");
                if (string.IsNullOrWhiteSpace(dto.Ruta)) missingFields.Add("Ruta");

                if (missingFields.Any())
                {
                    var fields = string.Join(", ", missingFields);
                    return Response<PermisoDto>.Fail($"Los siguientes campos son obligatorios: {fields}.");
                }

                // Mapear a entidad
                var entity = _mapper.Map<PermisoEnt>(dto);

                // Crear registro
                var created = await _permisoRepository.AddAsync(entity);

                if (created == null)
                    return Response<PermisoDto>.Fail("No se pudo crear el permiso.");

                // Mapear de vuelta a DTO
                var resultDto = _mapper.Map<PermisoDto>(created);

                return Response<PermisoDto>.Success(resultDto, "Permiso creado correctamente.");
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(AddPermisoUseCase)}.{nameof(ExecuteAsync)}", ex);
                return Response<PermisoDto>.Fail("Ocurrió un error inesperado al crear el permiso.");
            }
        }

    }
}
