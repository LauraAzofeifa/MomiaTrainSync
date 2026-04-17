using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.DTOs.UsuariosRoles
{
    public class PermisoDto
    {
        public int IdPermiso { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Ruta { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
}
