using MomiaTrainSync.Core.DTOs.UsuariosRoles;

namespace MomiaTrainSync.Web.ViewModels.UsuariosRoles
{
    public class ManageUsuariosViewModel
    {
        public IEnumerable<UsuarioDto>? Usuarios { get; set; }
        public UsuarioDto? UpdateUsuario { get; set; }
        public UsuarioDto? DeleteUsuario { get; set; }
    }
}
