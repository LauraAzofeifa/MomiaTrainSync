using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Domain.Entities
{
    public class UsuarioEnt
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string ContrasennaHash { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int RolId { get; set; }

        // Relaciones
        public RolEnt Rol { get; set; } = null!;
    }
}
