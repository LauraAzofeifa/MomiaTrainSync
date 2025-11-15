using AutoMapper;
using MomiaTrainSync.Core.Common;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Core.Interfaces.Repositories.UsuariosRoles;
using MomiaTrainSync.Core.Interfaces.Services;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;

namespace MomiaTrainSync.Core.UseCases.AuthenticationUseCase
{
    public class RegisterUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IRolRepository _rolRepository;
        private readonly ILogErrorRepository _logErrorRepository;
        private readonly IMapper _mapper;

        public RegisterUseCase(
            IUsuarioRepository usuarioRepository,
            IPasswordHasherService passwordHasherService,
            IRolRepository rolRepository,
            ILogErrorRepository logErrorRepository,
            IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasherService = passwordHasherService;
            _rolRepository = rolRepository;
            _logErrorRepository = logErrorRepository;
            _mapper = mapper;
        }

        public async Task<Response<UsuarioDto>> ExecuteAsync(string nombre, string apellido, string correo, string contrasena)
        {
            try
            {
                // Verificar si el correo ya existe
                var existingUser = await _usuarioRepository.GetByEmailAsync(correo);
                if (existingUser != null)
                    return Response<UsuarioDto>.Fail("El correo ya está registrado. Intenta con otro.");

                // Obtener el rol predeterminado (Atleta)
                var defaultRole = await _rolRepository.GetByNombreAsync("Atleta");
                if (defaultRole == null)
                    return Response<UsuarioDto>.Fail("El rol predeterminado no está configurado. Contacta al administrador.");

                // Hashear la contraseña
                var hash = _passwordHasherService.HashPassword(contrasena);

                // Crear la entidad
                var usuario = new UsuarioEnt
                {
                    Nombre = nombre,
                    Apellido = apellido,
                    Correo = correo,
                    ContrasennaHash = hash,
                    Estado = true,
                    FechaCreacion = DateTime.UtcNow,
                    RolId = defaultRole.IdRol
                };

                // Guardar en base de datos
                var createdUser = await _usuarioRepository.AddAsync(usuario);

                // Mapear a DTO
                var usuarioDto = _mapper.Map<UsuarioDto>(createdUser);

                return Response<UsuarioDto>.Success(usuarioDto, "Usuario registrado exitosamente.");
            }
            catch (Exception ex)
            {
                await _logErrorRepository.AddLogAsync($"{nameof(RegisterUseCase)}.{nameof(ExecuteAsync)}", ex);
                return Response<UsuarioDto>.Fail("Ocurrió un error al registrar el usuario. Intenta más tarde.");
            }
        }
    }
}
