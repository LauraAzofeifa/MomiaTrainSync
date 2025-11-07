using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Domain.Entities.RutinasAsignaciones
{
    public class RutinaEnt
    {
        public int IdRutina { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public bool Estado { get; set; }

        public ICollection<AsignacionRutinaEnt> Asignaciones { get; set; } = new List<AsignacionRutinaEnt>();
    }
}
