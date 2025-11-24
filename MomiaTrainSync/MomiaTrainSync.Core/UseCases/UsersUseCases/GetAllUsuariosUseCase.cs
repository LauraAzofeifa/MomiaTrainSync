using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;

namespace MomiaTrainSync.Core.UseCases.UsersUseCases
{
    public class GetUsuariosUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogErrorRepository _logErrorRepository;
        private readonly IMapper _mapper;

        public GetUsuariosUseCase(
            IUsuarioRepository usuarioRepository,
            ILogErrorRepository logErrorRepository,
            IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _logErrorRepository = logErrorRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<UsuarioDto>>> ExecuteAsync(
            string? filtro = null,
            string? rol = null,
            bool incluirInactivos = false,
            int? id = null,
            int? entrenadorId = null)
        {
            try
            {
                IEnumerable<UsuarioEnt> usuarios;

                if (id.HasValue)
                {
                    var usuario = await _usuarioRepository.GetByIdWithRolAsync(id.Value);
                    usuarios = usuario != null
                        ? new[] { usuario }
                        : Array.Empty<UsuarioEnt>();
                }
                else if (entrenadorId.HasValue)
                {
                    usuarios = await _usuarioRepository.GetAtletasByEntrenadorAsync(
                        entrenadorId.Value,
                        incluirInactivos
                    );
                }
                else
                {
                    usuarios = await _usuarioRepository.GetAllAsync(
                        asNoTracking: true,
                        includeInactive: incluirInactivos
                    );

                    if (!string.IsNullOrWhiteSpace(filtro))
                        usuarios = usuarios.Where(u =>
                            u.Nombre.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                            u.Correo.Contains(filtro, StringComparison.OrdinalIgnoreCase));

                    if (!string.IsNullOrWhiteSpace(rol))
                        usuarios = usuarios.Where(u =>
                            u.Rol != null &&
                            u.Rol.Nombre.Equals(rol, StringComparison.OrdinalIgnoreCase));
                }

                var usuariosDto = _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);

                if (!usuariosDto.Any())
                    return Response<IEnumerable<UsuarioDto>>.Fail("No se encontraron usuarios.");

                return Response<IEnumerable<UsuarioDto>>
                    .Success(usuariosDto, "Usuarios obtenidos correctamente.");
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync(
                    $"{nameof(GetUsuariosUseCase)}.{nameof(ExecuteAsync)}",
                    ex
                );

                return Response<IEnumerable<UsuarioDto>>.Fail(
                    "Error al obtener los usuarios: " + ex.Message
                );
            }
        }
    }
}
