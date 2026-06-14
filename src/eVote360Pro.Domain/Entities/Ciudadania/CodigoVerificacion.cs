using eVote360Pro.Core.Domain.Common;

namespace eVote360Pro.Core.Domain.Entities.Ciudadania
{
    public class CodigoVerificacion : BaseEntity
    {
        public string Codigo { get; set; } = string.Empty;
        public DateTime FechaGeneracion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool Usado { get; set; } = false;

        // Navigation properties
        public int CiudadanoId { get; set; }
        public Ciudadano Ciudadano { get; set; } = null!;
        public int EleccionId { get; set; }
        public Elecciones.Eleccion Eleccion { get; set; } = null!;
    }
}
