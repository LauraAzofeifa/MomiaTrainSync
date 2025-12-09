using MomiaTrainSync.Core.DTOs.EntrenamientoZonas;

namespace MomiaTrainSync.Web.ViewModels.ZonasEntrenamiento
{
    public class ZonaEntrenameintoViewModel
    {
        public IEnumerable<ZonaEntrenamientoDto> ZonasEntrenamientos { get; set; } = Enumerable.Empty<ZonaEntrenamientoDto>();

        public ZonaEntrenamientoDto NuevaZonaEntrenamiento { get; set; } = new ZonaEntrenamientoDto();

        public ZonaEntrenamientoForm UpdateZonaEntrenamiento { get; set; } = new ZonaEntrenamientoForm();
    }


    public class ZonaEntrenamientoForm
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Factor { get; set; }
    }

}
