using Microsoft.AspNetCore.Mvc.Rendering;
using MomiaTrainSync.Core.DTOs.UsuariosRoles;

namespace MomiaTrainSync.Web.ViewModels.UsuariosRoles
{
    public enum EstadoUsuarioFiltro
    {
        Todos = 0,
        SoloActivos = 1,
        SoloInactivos = 2
    }

    public class UserFilterRequest
    {
        public EstadoUsuarioFiltro Estado { get; set; } = EstadoUsuarioFiltro.Todos;

        public string? RolSeleccionado { get; set; }
    }

    public class ManageUsuariosViewModel
    {
        public IEnumerable<UsuarioDto>? Usuarios { get; set; }

        public UserFilterRequest Filtro { get; set; } = new();

        // Roles used for the filter (value = role name)
        public IEnumerable<SelectListItem>? Roles { get; set; }

        // Roles used for edit/select (value = role id)
        public IEnumerable<SelectListItem>? RolesForEdit { get; set; }
    }

}
