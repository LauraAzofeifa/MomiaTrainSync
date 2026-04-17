using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Core.UseCases.Base;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;

namespace MomiaTrainSync.Core.UseCases.UsersUseCases
{
    public class GetUsuariosUseCase : BaseUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public GetUsuariosUseCase(
            IUsuarioRepository usuarioRepository,
            ILogErrorRepository logErrorRepository,
            IMapper mapper
            ) : base (mapper, logErrorRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Response<IEnumerable<UsuarioDto>>> ExecuteAsync(
            string? filtro = null,
            string? rol = null,
            bool incluirInactivos = false,
            int? id = null,
            int? entrenadorId = null)
        {

            return await HandleAsync(
                async () =>
                {
                    IEnumerable<UsuarioEnt> usuarios;

                    // ========================
                    //   BUSCAR POR ID
                    // ========================
                    if (id.HasValue)
                    {
                        var usuario = await _usuarioRepository.GetByIdWithRolAsync(id.Value);

                        usuarios = usuario != null
                            ? new[] { usuario }
                            : Enumerable.Empty<UsuarioEnt>();

                        return BuildResult(usuarios, "Usuario obtenido correctamente.");
                    }

                    // ===========================
                    //   BUSCAR ATLETAS POR ENTRENADOR
                    // ===========================
                    if (entrenadorId.HasValue)
                    {
                        usuarios = await _usuarioRepository.GetAtletasByEntrenadorAsync(
                            entrenadorId.Value,
                            incluirInactivos);

                        return BuildResult(usuarios, "Usuarios obtenidos correctamente.");
                    }

                    // ===========================
                    //   OBTENER TODOS
                    // ===========================
                    usuarios = await _usuarioRepository.GetAllAsync(
                        asNoTracking: true,
                        includeInactive: incluirInactivos
                    );

                    // ---------------------------
                    //     FILTRO TEXTO
                    // ---------------------------
                    if (!string.IsNullOrWhiteSpace(filtro))
                    {
                        usuarios = usuarios.Where(u =>
                            (u.Nombre?.Contains(filtro, StringComparison.OrdinalIgnoreCase) ?? false) ||
                            (u.Correo?.Contains(filtro, StringComparison.OrdinalIgnoreCase) ?? false));
                    }

                    // ---------------------------
                    //     FILTRO POR ROL
                    // ---------------------------
                    if (!string.IsNullOrWhiteSpace(rol))
                    {
                        usuarios = usuarios.Where(u =>
                            u.Rol != null &&
                            u.Rol.Nombre.Equals(rol, StringComparison.OrdinalIgnoreCase));
                    }

                    return BuildResult(usuarios, "Usuarios obtenidos correctamente.");
                }
            );
            
        }

        // ======================================
        //      MÉTODO PRIVADO PARA ARMAR RESPUESTA
        // ======================================
        private Response<IEnumerable<UsuarioDto>> BuildResult(
            IEnumerable<UsuarioEnt> usuarios,
            string successMessage)
        {
            var usuariosDto = _mapper!.Map<IEnumerable<UsuarioDto>>(usuarios);

            if (!usuariosDto.Any())
                return Response<IEnumerable<UsuarioDto>>.Fail("No se encontraron usuarios.");

            return Response<IEnumerable<UsuarioDto>>.Success(usuariosDto, successMessage);
        }
    }
}
