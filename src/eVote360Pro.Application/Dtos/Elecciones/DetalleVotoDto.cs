namespace eVote360Pro.Core.Application.Dtos.Elecciones
{
    public class DetalleVotoDto
    {
        public int Id { get; set; }
        public int VotoId { get; set; }
        public int CandidatoId { get; set; }
        public string NombreCandidato { get; set; } = string.Empty;
        public string ApellidoCandidato { get; set; } = string.Empty;
        public int PartidoPoliticoId { get; set; }
        public string NombrePartido { get; set; } = string.Empty;
        public int PuestoElectivoId { get; set; }
        public string NombrePuesto { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
