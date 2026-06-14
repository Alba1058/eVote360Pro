using eVote360Pro.Core.Domain.Common;
using eVote360Pro.Core.Domain.Enums;

namespace eVote360Pro.Core.Domain.Entities.Partidos
{
    public class AsignacionCandidatoPuesto : BaseEntity
    {
        public TipoCandidatura TipoCandidatura { get; set; }

        public int CandidatoId { get; set; }
        public Candidato Candidato { get; set; } = null!;
        public int PuestoElectivoId { get; set; }
        public Puestos.PuestoElectivo PuestoElectivo { get; set; } = null!;
        public int PartidoPoliticoId { get; set; }
        public PartidoPolitico PartidoPolitico { get; set; } = null!;
    }
}
