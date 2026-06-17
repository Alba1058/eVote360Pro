namespace eVote360Pro.Core.Application.Dtos.Elecciones
{
    public class PuestoVotacionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int CantidadPartidos { get; set; }
        public int CantidadCandidatos { get; set; }
        public bool Seleccionado { get; set; }
    }
}
