using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomiaTrainSync.Domain.Entities.EntrenadorAtleta;
using MomiaTrainSync.Domain.Entities.EntrenamientosZonas;

namespace MomiaTrainSync.Domain.Entities.RutinasAsignaciones
{
    public class RutinaEnt
    {
        public int IdRutina { get; set; }
        public int IdRelacion { get; set; } // FK
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public bool Estado { get; set; }

        // Relaciones
        public EntrenadorAtletaEnt? Relacion { get; set; }  // relación entrenador-atleta
        public ICollection<EntrenamientoEnt> Entrenamientos { get; set; } = new List<EntrenamientoEnt>(); 
    }
}
