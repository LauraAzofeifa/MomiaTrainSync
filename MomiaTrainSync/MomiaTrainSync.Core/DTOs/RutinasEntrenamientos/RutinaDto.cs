using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MomiaTrainSync.Core.DTOs.EntrenamientoZonas;

namespace MomiaTrainSync.Core.DTOs.RutinasAsignaciones
{
    public class RutinaDto
    {
        public int IdRutina { get; set; }
        public int IdRelacion { get; set; } // FK
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public bool Estado { get; set; }

        public List<EntrenamientoDto> Entrenamientos { get; set; } = new();
    }
}
