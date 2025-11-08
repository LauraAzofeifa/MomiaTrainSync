using System.ComponentModel.DataAnnotations;

namespace MomiaTrainSync.Web.ViewModels
{
    public class RecoverPasswordViewModel
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        public string Correo { get; set; } = string.Empty;
    }
}
