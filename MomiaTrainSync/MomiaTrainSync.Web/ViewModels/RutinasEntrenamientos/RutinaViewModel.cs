using MomiaTrainSync.Core.DTOs.RutinasEntrenamientos;
using System.ComponentModel.DataAnnotations;

namespace MomiaTrainSync.Web.ViewModels.RutinasEntrenamientos
{
    public class RutinaViewModel
    {
        public int IdRelacion { get; set; }
        public int? IdRutina { get; set; }
        public IEnumerable<RutinaDto>? Rutinas { get; set; }
        public RutinaFormViewModel RutinaFormCreate { get; set; } = new RutinaFormViewModel();
        public RutinaFormViewModel RutinaFormUpdate { get; set; } = new RutinaFormViewModel();

    }

    public class RutinaFormViewModel 
    {
        public int? IdRutina { get; set; }
        public int? IdRelacion { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(150, ErrorMessage = "El nombre no puede exceder los 150 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(255, ErrorMessage = "El nombre no puede exceder los 255 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        public bool Estado { get; set; } = true;
    }
}
