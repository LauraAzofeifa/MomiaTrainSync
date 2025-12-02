using MomiaTrainSync.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Domain.Entities.UsuariosRoles
{
    public class PermisoEnt : ISoftDelete
    {
        public int IdPermiso { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Ruta { get; set; } = string.Empty;
        public bool Estado { get; set; }

        public ICollection<RolPermisoEnt> RolPermisos { get; set; } = new List<RolPermisoEnt>();
    }
}
