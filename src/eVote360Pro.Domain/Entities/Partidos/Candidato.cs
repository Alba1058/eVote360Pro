using eVote360Pro.Core.Domain.Common;

namespace eVote360Pro.Core.Domain.Entities.Partidos
{
    public class Candidato : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Foto { get; set; }

        public int PartidoPoliticoId { get; set; }
        public PartidoPolitico PartidoPolitico { get; set; } = null!;
        public ICollection<AsignacionCandidatoPuesto> AsignacionesPuestos { get; set; } = new List<AsignacionCandidatoPuesto>();
    }
}
