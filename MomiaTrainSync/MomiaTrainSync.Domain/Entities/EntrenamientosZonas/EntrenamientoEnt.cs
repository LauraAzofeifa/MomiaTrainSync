using MomiaTrainSync.Domain.Entities.RutinasAsignaciones;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Domain.Entities.EntrenamientosZonas
{
    public class EntrenamientoEnt
    {
        public int IdEntrenamiento { get; set; }
        public int IdEntrenador { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string TipoSesion { get; set; } = string.Empty;
        public string Objetivo { get; set; } = string.Empty;
        public int DuracionEstimada { get; set; }
        public byte NivelEsfuerzoEsperado { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Relaciones
        public UsuarioEnt? Entrenador { get; set; }
        public ICollection<DetalleZonaPlanEnt> DetallesZonaPlan { get; set; } = new List<DetalleZonaPlanEnt>();
        public ICollection<AsignacionRutinaEnt> Asignaciones { get; set; } = new List<AsignacionRutinaEnt>();
    }
}
