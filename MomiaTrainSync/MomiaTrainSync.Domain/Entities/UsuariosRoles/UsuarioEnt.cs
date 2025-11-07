using MomiaTrainSync.Domain.Entities.EntrenadorAtleta;
using MomiaTrainSync.Domain.Entities.EntrenamientosZonas;
using System;

namespace MomiaTrainSync.Domain.Entities.UsuariosRoles
{
    public class UsuarioEnt
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public DateTime? FechaCumpleannos { get; set; }
        public string ContrasennaHash { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int RolId { get; set; }

        // Relaciones
        public RolEnt? Rol { get; set; }
        public ICollection<EntrenadorAtletaEnt> EntrenamientosComoEntrenador { get; set; } = new List<EntrenadorAtletaEnt>();
        public ICollection<EntrenadorAtletaEnt> EntrenamientosComoAtleta { get; set; } = new List<EntrenadorAtletaEnt>();
        public ICollection<EntrenamientoEnt> EntrenamientosCreados { get; set; } = new List<EntrenamientoEnt>();
    }
}
