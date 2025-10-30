using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs;
using MomiaTrainSync.Core.Interfaces.Repositories;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.UseCases.Authentication;
using MomiaTrainSync.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.UseCases
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
            int? id = null)
        {
            try
            {
                IEnumerable<UsuarioEnt> usuarios;

                if (id.HasValue)
                {
                    // Buscar usuario por ID
                    var usuario = await _usuarioRepository.GetByIdAsync(id.Value);
                    usuarios = usuario != null ? new List<UsuarioEnt> { usuario } : new List<UsuarioEnt>();
                }
                else
                {
                    // Obtener lista general
                    usuarios = await _usuarioRepository.GetAllAsync();

                    if (!incluirInactivos)
                        usuarios = usuarios.Where(u => u.Estado);

                    if (!string.IsNullOrEmpty(filtro))
                        usuarios = usuarios.Where(u =>
                            u.Nombre.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                            u.Correo.Contains(filtro, StringComparison.OrdinalIgnoreCase));

                    if (!string.IsNullOrEmpty(rol))
                        usuarios = usuarios.Where(u => u.Rol.Nombre.Equals(rol, StringComparison.OrdinalIgnoreCase));
                }

                var usuariosDto = _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);

                if (!usuariosDto.Any())
                    return Response<IEnumerable<UsuarioDto>>.Fail("No se encontraron usuarios.");

                return Response<IEnumerable<UsuarioDto>>.Success(usuariosDto, "Usuarios obtenidos correctamente.");
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(GetUsuariosUseCase)}.{nameof(ExecuteAsync)}", ex);
                return Response<IEnumerable<UsuarioDto>>.Fail("Error al obtener los usuarios: " + ex.Message);
            }
        }
    }

}
