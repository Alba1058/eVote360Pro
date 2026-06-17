namespace eVote360Pro.Core.Application.Dtos.Elecciones
{
    public class CandidatoVotacionDto
    {
        public int Id { get; set; }
        public int AsignacionId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Foto { get; set; } = string.Empty;
        public string PartidoNombre { get; set; } = string.Empty;
        public string PartidoSiglas { get; set; } = string.Empty;
        public string PartidoLogo { get; set; } = string.Empty;
        public bool EsNinguno { get; set; }
    }
}
