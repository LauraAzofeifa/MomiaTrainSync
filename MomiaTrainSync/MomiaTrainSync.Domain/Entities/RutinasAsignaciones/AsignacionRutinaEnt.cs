using MomiaTrainSync.Domain.Entities.EntrenadorAtleta;
using MomiaTrainSync.Domain.Entities.EntrenamientosZonas;
using MomiaTrainSync.Domain.Entities.SesionesEntrenamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Domain.Entities.RutinasAsignaciones
{
    public class AsignacionRutinaEnt
    {
        public int IdAsignacion { get; set; }
        public int IdRutina { get; set; }
        public int IdEntrenamiento { get; set; }
        public int IdRelacion { get; set; }
        public DateTime FechaProgramada { get; set; }
        public string? NotaEntrenador { get; set; }
        public string Estado { get; set; } = string.Empty;

        // Relaciones
        public RutinaEnt? Rutina { get; set; }
        public EntrenamientoEnt? Entrenamiento { get; set; }
        public EntrenadorAtletaEnt? Relacion { get; set; }
        public ICollection<SesionEntrenamientoEnt> Sesiones { get; set; } = new List<SesionEntrenamientoEnt>();
    }
}
