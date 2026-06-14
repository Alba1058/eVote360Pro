using eVote360Pro.Core.Domain.Enums;

namespace eVote360Pro.Core.Application.Dtos.Partidos
{
    public class AsignacionCandidatoPuestoDto
    {
        public int Id { get; set; }
        public int CandidatoId { get; set; }
        public string NombreCandidato { get; set; } = string.Empty;
        public string ApellidoCandidato { get; set; } = string.Empty;
        public int PuestoElectivoId { get; set; }
        public string NombrePuesto { get; set; } = string.Empty;
        public int PartidoPoliticoId { get; set; }
        public string NombrePartido { get; set; } = string.Empty;
        public TipoCandidatura TipoCandidatura { get; set; }
        public bool IsActive { get; set; }
    }
}
