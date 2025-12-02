using MomiaTrainSync.Domain.Common;
using MomiaTrainSync.Domain.Entities.RutinasAsignaciones;
using MomiaTrainSync.Domain.Entities.UsuariosRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Domain.Entities.EntrenadorAtleta
{
    public class EntrenadorAtletaEnt : ISoftDelete
    {
        public int IdRelacion { get; set; }
        public int IdEntrenador { get; set; }
        public int IdAtleta { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public bool Estado { get; set; }

        // Relaciones
        public UsuarioEnt? Entrenador { get; set; }
        public UsuarioEnt? Atleta { get; set; }
        public ICollection<RutinaEnt> Rutinas { get; set; } = new List<RutinaEnt>();
    }
}
