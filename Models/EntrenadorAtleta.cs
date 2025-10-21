namespace MomiaTrainSync.Models
{
    public class EntrenadorAtleta
    {
        public int Id { get; set; }
        public int EntrenadorId { get; set; }
        public int AtletaId { get; set; }
        public Usuario Entrenador { get; set; }
        public Usuario Atleta { get; set; }
    }
}