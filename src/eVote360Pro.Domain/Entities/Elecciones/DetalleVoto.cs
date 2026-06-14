using eVote360Pro.Core.Domain.Common;

namespace eVote360Pro.Core.Domain.Entities.Elecciones
{
    public class DetalleVoto : BaseEntity
    {
        public bool VotoNulo { get; set; } = false;

        // Navigation properties
        public int VotoId { get; set; }
        public Voto Voto { get; set; } = null!;
        public int? CandidatoId { get; set; }
        public Partidos.Candidato? Candidato { get; set; }
        public int PuestoElectivoId { get; set; }
        public Puestos.PuestoElectivo PuestoElectivo { get; set; } = null!;
    }
}
