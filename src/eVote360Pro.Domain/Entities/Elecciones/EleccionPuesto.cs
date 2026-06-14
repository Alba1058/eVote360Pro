using eVote360Pro.Core.Domain.Common;

namespace eVote360Pro.Core.Domain.Entities.Elecciones
{
    public class EleccionPuesto : BaseEntity
    {
        // Navigation properties
        public int EleccionId { get; set; }
        public Eleccion Eleccion { get; set; } = null!;
        public int PuestoElectivoId { get; set; }
        public Puestos.PuestoElectivo PuestoElectivo { get; set; } = null!;
    }
}
