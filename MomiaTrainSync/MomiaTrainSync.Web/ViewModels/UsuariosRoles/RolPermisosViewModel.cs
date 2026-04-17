using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using System.ComponentModel.DataAnnotations;

namespace MomiaTrainSync.Web.ViewModels.UsuariosRoles
{
    public class RolPermisosViewModel
    {
        // Lista de todos los roles disponibles
        public List<RolDto> Roles { get; set; } = new();

        // Rol seleccionado actualmente
        public int? RolSeleccionadoId { get; set; }

        // Lista de todos los permisos del sistema
        public List<PermisoDto> TodosPermisos { get; set; } = new();

        // Permisos actualmente asignados al rol seleccionado
        public List<int> PermisosAsignadosIds { get; set; } = new();

        // Para manejar nuevas asignaciones desde la vista
        public List<int> PermisosSeleccionados { get; set; } = new();

        // Sub-ViewModels
        public RolFormViewModel RolForm { get; set; } = new();
        public PermisoFormViewModel PermisoFormAdd { get; set; } = new();
        public PermisoFormViewModel PermisoFormEdit { get; set; } = new();
    }



    public class RolFormViewModel
    {
        public int IdRol { get; set; }

        [Required(ErrorMessage = "El nombre del rol es requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El nombre del rol es requerido")]
        public string Descripcion { get; set; }

        public bool Estado { get; set; }
    }

    public class PermisoFormViewModel
    {
        public int IdPermiso { get; set; }

        [Required(ErrorMessage = "El código del permiso es requerido")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "La descripción del permiso es requerida")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La categoría del permiso es requerida")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "La ruta del permiso es requerida")]
        public string Ruta { get; set; }

        public bool Estado { get; set; }
    }
}
