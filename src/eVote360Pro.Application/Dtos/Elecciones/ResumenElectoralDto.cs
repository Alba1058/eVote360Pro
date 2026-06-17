namespace eVote360Pro.Core.Application.Dtos.Elecciones
{
    public class ResumenElectoralDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public int CantidadPartidos { get; set; }
        public int CantidadCandidatos { get; set; }
        public int CantidadCiudadanosVotaron { get; set; }
        public int CantidadPuestos { get; set; }
    }
}
