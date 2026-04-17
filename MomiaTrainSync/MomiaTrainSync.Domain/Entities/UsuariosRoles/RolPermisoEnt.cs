using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Domain.Entities.UsuariosRoles
{
    public class RolPermisoEnt
    {
        public int IdRol { get; set; }
        public int IdPermiso { get; set; }

        // Relaciones
        public RolEnt? Rol { get; set; }
        public PermisoEnt? Permiso { get; set; }
    }
}
