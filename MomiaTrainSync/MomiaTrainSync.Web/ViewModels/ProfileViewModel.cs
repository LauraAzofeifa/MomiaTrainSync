using MomiaTrainSync.Core.DTOs.UsuariosRoles;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MomiaTrainSync.Web.ViewModels
{
    public class ProfileViewModel
    {
        public UsuarioDto Details { get; set; } = new();
        public UpdateProfileViewModel Update { get; set; } = new();
        public ChangePasswordViewModel ChangePassword { get; set; } = new();
    }

    public class UpdateProfileViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [RegularExpression(@"^(?=.{2,50}$)(?!.*\s{2,})([A-ZÁÉÍÓÚÑ][a-záéíóúñ]+)(\s[A-ZÁÉÍÓÚÑ][a-záéíóúñ]+)*$",
    ErrorMessage = "El nombre solo puede contener letras y espacios, debe de iniciar con mayúscula.")]
        public string Nombre { get; set; } = string.Empty;


        [Required(ErrorMessage = "El apellido es requerido")]
        [RegularExpression(@"^(?=.{2,50}$)(?!.*\s{2,})([A-ZÁÉÍÓÚÑ][a-záéíóúñ]+)(\s[A-ZÁÉÍÓÚÑ][a-záéíóúñ]+)*$",
            ErrorMessage = "El apellido solo puede contener letras y espacios, debe de iniciar con mayúscula.")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El telefono es requerido")]
        [Phone(ErrorMessage = "Debe ingresar un número de teléfono válido")]
        [RegularExpression(@"^[0-9]{8}$", ErrorMessage = "El formato es inválido, debe ser XXXXXXXX")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La biografía es requerida")]
        [StringLength(255)]
        public string Biografia { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? FechaCumpleannos { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "La contraseña es requerida")]
        [DisplayName("Contraseña Actual")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [RegularExpression(@"^(?=\S+$)(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{6,100}$",
            ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres, incluir mayúscula, minúscula, número, carácter especial y no contener espacios.")]
        [DisplayName("Nueva Contraseña")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden")]
        [DisplayName("Confirmar Nueva Contraseña")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
