using MomiaTrainSync.Core.DTOs;
using MomiaTrainSync.Core.Interfaces.Repositories;
using MomiaTrainSync.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.UseCases
{
    public class LoginUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasherService _passwordHasherService;

        public LoginUseCase(IUsuarioRepository usuarioRepository, IPasswordHasherService passwordHasherService)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasherService = passwordHasherService;
        }

        public async Task<UsuarioDto?> ExecuteAsync(string correo, string contrasena)
        {
            // Buscar usuario por correo
            var usuario = await _usuarioRepository.GetByEmailAsync(correo);
            if (usuario == null) return null;

            // Validar contraseña (puede estar hasheada)
            if (!_passwordHasherService.VerifyPassword(usuario.ContrasennaHash, contrasena))
                return null;

            // Mapear a DTO para no exponer la entidad
            return new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Rol = usuario.Rol.Nombre
            };
        }
    }
}
