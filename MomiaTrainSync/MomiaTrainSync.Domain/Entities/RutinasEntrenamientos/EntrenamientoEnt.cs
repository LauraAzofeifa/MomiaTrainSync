using MomiaTrainSync.Domain.Common;
using MomiaTrainSync.Domain.Entities.RutinasAsignaciones;
using MomiaTrainSync.Domain.Entities.SesionesEntrenamiento;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Domain.Entities.EntrenamientosZonas
{
    public class EntrenamientoEnt : ISoftDelete
    {
        public int IdEntrenamiento { get; set; }
        public int IdRutina { get; set; } // FK
        public string Nombre { get; set; } = string.Empty;
        public string TipoSesion { get; set; } = string.Empty;
        public string Objetivo { get; set; } = string.Empty;
        public int DuracionEstimada { get; set; }
        public byte NivelEsfuerzoEsperado { get; set; }
        public string? Descripcion { get; set; }
        public DateOnly FechaProgramada { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Estado { get; set; }

        // Relaciones
        public RutinaEnt? Rutina { get; set; }
        public ICollection<DetalleZonaPlanEnt> DetallesZonaPlan { get; set; } = new List<DetalleZonaPlanEnt>();
        public ICollection<SesionEntrenamientoEnt> Sesiones { get; set; } = new List<SesionEntrenamientoEnt>(); 
    }
}
