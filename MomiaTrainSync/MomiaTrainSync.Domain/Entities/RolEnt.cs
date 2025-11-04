using MomiaTrainSync.Domain.Entities;
using System.Collections.Generic;

namespace MomiaTrainSync.Domain.Entities
{
    public class RolEnt
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        // Relación inversa con Usuario
        public ICollection<UsuarioEnt> Usuarios { get; set; } = new List<UsuarioEnt>();

        // Relación N:M con Permisos
        public ICollection<RolPermisoEnt> RolPermisos { get; set; } = new List<RolPermisoEnt>();
    }

    public class PermisoEnt
    {
        public int IdPermiso { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Ruta { get; set; } = string.Empty;
        public bool Estado { get; set; }

        public ICollection<RolPermisoEnt> RolPermisos { get; set; } = new List<RolPermisoEnt>();
    }

    public class RolPermisoEnt
    {
        public int IdRol { get; set; }
        public RolEnt Rol { get; set; } = null!;

        public int IdPermiso { get; set; }
        public PermisoEnt Permiso { get; set; } = null!;
    }
}
