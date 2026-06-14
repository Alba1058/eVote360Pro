using eVote360Pro.Core.Domain.Enums;

namespace eVote360Pro.Core.Application.Dtos.Partidos
{
    public class SaveAsignacionCandidatoPuestoDto
    {
        public int Id { get; set; }
        public required int CandidatoId { get; set; }
        public required int PuestoElectivoId { get; set; }
        public required int PartidoPoliticoId { get; set; }
        public required TipoCandidatura TipoCandidatura { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
