using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
        public bool Estado { get; set; } = false;
        public DateTime FechaIngreso { get; set; }
    }
}
