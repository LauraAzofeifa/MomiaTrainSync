using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
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
    public class GetPermisosUseCase : BaseUseCase
    {
        private readonly IPermisoRepository _permisoRepository;

        public GetPermisosUseCase(
            IPermisoRepository permisoRepository,
            ILogErrorRepository logError,
            IMapper mapper
            ) : base(mapper, logError)
        {
            _permisoRepository = permisoRepository;
        }

        public async Task<Response<IEnumerable<PermisoDto>>> ExecuteAsync(
            int? id = null,
            string? codigo = null,
            string? categoria = null,
            bool incluirInactivos = true)
        {
            return await HandleAsync(async () =>
            {
                IEnumerable<PermisoEnt> permisos;

                // Buscar por ID
                if (id.HasValue)
                {
                    var permiso = await _permisoRepository.GetByIdAsync(id.Value);
                    permisos = permiso != null ? new[] { permiso } : Enumerable.Empty<PermisoEnt>();
                }
                // Buscar por Código
                else if (!string.IsNullOrWhiteSpace(codigo))
                {
                    if (incluirInactivos)
                    {
                        var todos = await _permisoRepository.GetAllAsync(includeInactive: incluirInactivos);
                        permisos = todos.Where(p => p.Codigo == codigo);
                    }
                    else
                    {
                        var permiso = await _permisoRepository.GetByCodigoAsync(codigo);
                        permisos = permiso != null ? new[] { permiso } : Enumerable.Empty<PermisoEnt>();
                    }
                }
                // Filtrar por Categoría
                else if (!string.IsNullOrWhiteSpace(categoria))
                {
                    if (incluirInactivos)
                    {
                        var todos = await _permisoRepository.GetAllAsync(true);
                        permisos = todos.Where(p => p.Categoria == categoria);
                    }
                    else
                    {
                        permisos = await _permisoRepository.GetByCategoriaAsync(categoria);
                    }
                }
                // Obtener todos
                else
                {
                    permisos = await _permisoRepository.GetAllAsync(includeInactive: incluirInactivos);
                }

                var permisosDto = _mapper.Map<IEnumerable<PermisoDto>>(permisos);

                if (!permisosDto.Any())
                    return Response<IEnumerable<PermisoDto>>.Fail("No se encontraron permisos.");

                return Response<IEnumerable<PermisoDto>>.Success(
                    permisosDto,
                    "Permisos obtenidos correctamente."
                );
            });
        }
    }
}
