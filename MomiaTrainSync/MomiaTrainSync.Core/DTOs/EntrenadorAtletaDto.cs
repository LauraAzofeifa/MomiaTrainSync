using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.DTOs
{
    public class EntrenadorAtletaDto
    {
        public int IdRelacion { get; set; }
        public int IdEntrenador { get; set; }
        public int IdAtleta { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public bool Estado { get; set; }

        public UsuarioDto? Entrenador { get; set; }
        public UsuarioDto? Atleta { get; set; }
    }
}
