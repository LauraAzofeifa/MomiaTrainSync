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
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public DateTime FechaCumpleannos { get; set; }
        public string ContrasennaHash { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int RolId { get; set; }
        public RolDto? Rol { get; set; }
    }

    public class RolDto
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        public List<PermisoDto> Permisos { get; set; } = new();
    }

    public class PermisoDto
    {
        public int IdPermiso { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Ruta { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }

    public class RolPermisoDto
    {
        public int IdRol { get; set; }
        public int IdPermiso { get; set; }
    }
}
