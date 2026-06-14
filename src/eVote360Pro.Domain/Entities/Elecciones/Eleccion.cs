using eVote360Pro.Core.Domain.Common;
using eVote360Pro.Core.Domain.Enums;

namespace eVote360Pro.Core.Domain.Entities.Elecciones
{
    public class Eleccion : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public EstadoEleccion Estado { get; set; }

        // Navigation properties
        public ICollection<EleccionPuesto> EleccionesPuestos { get; set; } = new List<EleccionPuesto>();
        public ICollection<Voto> Votos { get; set; } = new List<Voto>();
    }
}
