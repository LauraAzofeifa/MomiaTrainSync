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

namespace MomiaTrainSync.Core.UseCases.RolesPermisos
{
    public class GetPermisosUseCase
    {
        private readonly IPermisoRepository _permisoRepository;
        private readonly ILogErrorRepository _logErrorRepository;
        private readonly IMapper _mapper;

        public GetPermisosUseCase(
            IPermisoRepository permisoRepository,
            ILogErrorRepository logErrorRepository,
            IMapper mapper)
        {
            _permisoRepository = permisoRepository;
            _logErrorRepository = logErrorRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<PermisoDto>>> ExecuteAsync(
            int? id = null,
            string? codigo = null,
            string? categoria = null,
            bool incluirInactivos = false)
        {
            try
            {
                IEnumerable<PermisoEnt> permisos;

                // 🔹 Buscar por ID
                if (id.HasValue)
                {
                    var permiso = await _permisoRepository.GetByIdAsync(id.Value);
                    permisos = permiso != null ? new List<PermisoEnt> { permiso } : Enumerable.Empty<PermisoEnt>();
                }
                // 🔹 Buscar por código
                else if (!string.IsNullOrEmpty(codigo))
                {
                    var permiso = await _permisoRepository.GetByCodigoAsync(codigo);
                    permisos = permiso != null ? new List<PermisoEnt> { permiso } : Enumerable.Empty<PermisoEnt>();
                }
                // 🔹 Filtrar por categoría
                else if (!string.IsNullOrEmpty(categoria))
                {
                    permisos = await _permisoRepository.GetByCategoriaAsync(categoria);
                    if (incluirInactivos)
                    {
                        // Traemos también inactivos si se requiere
                        var todos = await _permisoRepository.GetAllAsync(true);
                        permisos = todos.Where(p => p.Categoria == categoria);
                    }
                }
                // 🔹 Obtener todos
                else
                {
                    permisos = await _permisoRepository.GetAllAsync(incluirInactivos);
                }

                var permisosDto = _mapper.Map<IEnumerable<PermisoDto>>(permisos);

                if (!permisosDto.Any())
                    return Response<IEnumerable<PermisoDto>>.Fail("No se encontraron permisos.");

                return Response<IEnumerable<PermisoDto>>.Success(
                    permisosDto,
                    "Permisos obtenidos correctamente."
                );
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync(
                    $"{nameof(GetPermisosUseCase)}.{nameof(ExecuteAsync)}", ex);
                return Response<IEnumerable<PermisoDto>>.Fail(
                    "Error al obtener los permisos: " + ex.Message);
            }
        }
    }
}
