using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Domain.Entities.UsuariosRoles
{
    public class RolEnt
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Estado { get; set; }

        // Relaciones
        public ICollection<UsuarioEnt> Usuarios { get; set; } = new List<UsuarioEnt>();
        public ICollection<RolPermisoEnt> RolPermisos { get; set; } = new List<RolPermisoEnt>();
    }
}
