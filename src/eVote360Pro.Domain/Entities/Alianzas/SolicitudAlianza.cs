using eVote360Pro.Core.Domain.Common;
using eVote360Pro.Core.Domain.Enums;

namespace eVote360Pro.Core.Domain.Entities.Alianzas
{
    public class SolicitudAlianza : BaseEntity
    {
        public DateTime FechaSolicitud { get; set; }
        public EstadoSolicitudAlianza Estado { get; set; }

        public int PartidoSolicitanteId { get; set; }
        public Partidos.PartidoPolitico PartidoSolicitante { get; set; } = null!;
        public int PartidoReceptorId { get; set; }
        public Partidos.PartidoPolitico PartidoReceptor { get; set; } = null!;
    }
}
