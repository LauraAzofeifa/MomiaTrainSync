namespace MomiaTrainSync.Core.DTOs.UsuariosRoles
{
    public class UsuarioDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        // Perfil
        public string Telefono { get; set; } = string.Empty;

        public DateTime? FechaNacimiento { get; set; }

        public string? Biografia { get; set; }

        // Estado
        public bool Estado { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaUltimoCambioContrasenna { get; set; }
        public DateTime? FechaUltimoLogin { get; set; }

        // Rol
        public int RolId { get; set; }

        public RolDto? Rol { get; set; }
    }
}