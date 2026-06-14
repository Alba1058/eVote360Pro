using eVote360Pro.Core.Domain.Common;

namespace eVote360Pro.Core.Domain.Entities.Alianzas
{
    public class AlianzaPolitica : BaseEntity
    {
        public DateTime FechaAceptacion { get; set; }

        // Navigation properties
        public int Partido1Id { get; set; }
        public Partidos.PartidoPolitico Partido1 { get; set; } = null!;
        public int Partido2Id { get; set; }
        public Partidos.PartidoPolitico Partido2 { get; set; } = null!;
        public int SolicitudAlianzaId { get; set; }
        public SolicitudAlianza SolicitudAlianza { get; set; } = null!;
    }
}
