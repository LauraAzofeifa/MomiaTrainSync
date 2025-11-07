using MomiaTrainSync.Domain.Entities.RutinasAsignaciones;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Domain.Entities.EntrenadorAtleta
{
    public class EntrenadorAtletaEnt
    {
        public int IdRelacion { get; set; }
        public int IdEntrenador { get; set; }
        public int IdAtleta { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public bool Estado { get; set; }

        // Relaciones
        public UsuarioEnt? Entrenador { get; set; }
        public UsuarioEnt? Atleta { get; set; }
        public ICollection<AsignacionRutinaEnt> Asignaciones { get; set; } = new List<AsignacionRutinaEnt>();
    }
}
