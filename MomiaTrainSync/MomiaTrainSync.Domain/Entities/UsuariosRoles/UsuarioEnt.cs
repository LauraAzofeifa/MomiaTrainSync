using MomiaTrainSync.Domain.Common;
using MomiaTrainSync.Domain.Entities.EntrenadorAtleta;
using System;

namespace MomiaTrainSync.Domain.Entities.UsuariosRoles
{
    public class UsuarioEnt : ISoftDelete
    {
        #region Properties

        public int Id { get; set; }

        // Información básica (registro)
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;

        // Información de perfil
        public string Telefono { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }
        public string? Biografia { get; set; }

        // Seguridad
        public string ContrasennaHash { get; set; } = string.Empty;
        public DateTime? FechaUltimoCambioContrasenna { get; set; }
        public DateTime? FechaUltimoLogin { get; set; }

        // Estado del usuario
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Relaciones
        public int RolId { get; set; }
        public RolEnt? Rol { get; set; }

        public ICollection<EntrenadorAtletaEnt> EntrenamientosComoEntrenador { get; set; } = new List<EntrenadorAtletaEnt>();
        public ICollection<EntrenadorAtletaEnt> EntrenamientosComoAtleta { get; set; } = new List<EntrenadorAtletaEnt>();

        #endregion

        #region Constructors

        // 🔹 Constructor requerido por EF Core
        public UsuarioEnt()
        {
        }

        // 🔹 Constructor para registro
        public UsuarioEnt(
            string nombre,
            string apellido,
            string correo,
            string contrasennaHash,
            int rolId)
        {
            Nombre = nombre;
            Apellido = apellido;
            Correo = correo.Trim().ToLower();
            ContrasennaHash = contrasennaHash;
            RolId = rolId;

            Estado = true;
            FechaCreacion = DateTime.UtcNow;
            FechaUltimoCambioContrasenna = DateTime.UtcNow;
        }

        #endregion

        #region Methods

        // 🔹 Registrar login
        public void RegistrarLogin()
        {
            FechaUltimoLogin = DateTime.UtcNow;
        }

        // 🔹 Cambiar contraseña
        public void CambiarContrasenna(string nuevoHash)
        {
            ContrasennaHash = nuevoHash;
            FechaUltimoCambioContrasenna = DateTime.UtcNow;
        }

        // 🔹 Actualizar perfil
        public void ActualizarPerfil(
            string telefono,
            DateTime? fechaNacimiento,
            string? biografia)
        {
            Telefono = telefono;
            FechaNacimiento = fechaNacimiento;
            Biografia = biografia;
        }

        #endregion
    }
}