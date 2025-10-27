using MomiaTrainSync.Core.DTOs;
using MomiaTrainSync.Core.Interfaces.Repositories;
using MomiaTrainSync.Core.Interfaces.Services;
using MomiaTrainSync.Domain.Entities;
using MomiaTrainSync.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.UseCases
{
    public class RegisterUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasherService _passwordHasherService;

        public RegisterUseCase(IUsuarioRepository usuarioRepository, IPasswordHasherService passwordHasherService)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasherService = passwordHasherService;
        }

        public async Task<UsuarioDto?> ExecuteAsync(string nombre, string correo, string contrasena)
        {
            // 1. Verificar si el correo ya existe
            var existingUser = await _usuarioRepository.GetByEmailAsync(correo);
            if (existingUser != null) return null; // o lanzar excepción según tu preferencia

            // 2. Hashear la contraseña
            var hash = _passwordHasherService.HashPassword(contrasena);

            // 3. Crear la entidad
            var usuario = new UsuarioEnt
            {
                Nombre = nombre,
                Correo = correo,
                ContrasennaHash = hash,
                Estado = true, // por defecto activo
                FechaIngreso = DateTime.UtcNow,
                RolId = (int)RolEnum.Atleta
            };

            // 4. Guardar en la base de datos
            var createdUser = await _usuarioRepository.AddAsync(usuario);

            // 5. Mapear a DTO
            return new UsuarioDto
            {
                Id = createdUser.Id,
                Nombre = createdUser.Nombre,
                Correo = createdUser.Correo,
                Rol = ((RolEnum)createdUser.RolId).ToString()
            };
        }
    }
}
