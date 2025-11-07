using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs;
using MomiaTrainSync.Core.Interfaces.Repositories;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;

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

                // 🔹 1. Buscar usuario por ID (caso individual)
                if (id.HasValue)
                {
                    var usuario = await _usuarioRepository.GetByIdAsync(id.Value);
                    usuarios = usuario != null
                        ? new List<UsuarioEnt> { usuario }
                        : Enumerable.Empty<UsuarioEnt>();
                }
                // 🔹 2. Buscar atletas asignados a un entrenador
                else if (entrenadorId.HasValue)
                {
                    usuarios = await _usuarioRepository.GetAtletasByEntrenadorAsync(entrenadorId.Value, incluirInactivos);
                }
                // 🔹 3. Lista general
                else
                {
                    usuarios = await _usuarioRepository.GetAllAsync(incluirInactivos);

                    if (!string.IsNullOrEmpty(filtro))
                        usuarios = usuarios.Where(u =>
                            u.Nombre.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                            u.Correo.Contains(filtro, StringComparison.OrdinalIgnoreCase));

                    if (!string.IsNullOrEmpty(rol))
                        usuarios = usuarios.Where(u =>
                            u.Rol!.Nombre.Equals(rol, StringComparison.OrdinalIgnoreCase));
                }

                var usuariosDto = _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);

                if (!usuariosDto.Any())
                    return Response<IEnumerable<UsuarioDto>>.Fail("No se encontraron usuarios.");

                return Response<IEnumerable<UsuarioDto>>.Success(usuariosDto, "Usuarios obtenidos correctamente.");
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync(
                    $"{nameof(GetUsuariosUseCase)}.{nameof(ExecuteAsync)}", ex);
                return Response<IEnumerable<UsuarioDto>>.Fail("Error al obtener los usuarios: " + ex.Message);
            }
        }

    }

}
