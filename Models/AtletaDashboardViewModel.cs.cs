using MomiaTrainSync.Models;

public class AtletaDashboardViewModel
{
    public string NombreAtleta { get; set; }
    public RutinaItem RutinaDeHoy { get; set; }
    public List<Disponibilidad> Disponibilidades { get; set; }
    public List<ComentarioItem> Comentarios { get; set; }
}
