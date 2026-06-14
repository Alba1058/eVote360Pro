using eVote360Pro.Core.Domain.Common;

namespace eVote360Pro.Core.Domain.Entities.Elecciones
{
    public class Voto : BaseEntity
    {
        public DateTime FechaVoto { get; set; }

        // Navigation properties
        public int CiudadanoId { get; set; }
        public Ciudadania.Ciudadano Ciudadano { get; set; } = null!;
        public int EleccionId { get; set; }
        public Eleccion Eleccion { get; set; } = null!;
        public ICollection<DetalleVoto> DetallesVoto { get; set; } = new List<DetalleVoto>();
    }
}
