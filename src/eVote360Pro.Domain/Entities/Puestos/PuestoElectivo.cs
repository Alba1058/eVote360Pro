using eVote360Pro.Core.Domain.Common;

namespace eVote360Pro.Core.Domain.Entities.Puestos
{
    public class PuestoElectivo : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<Elecciones.EleccionPuesto> EleccionesPuestos { get; set; } = new List<Elecciones.EleccionPuesto>();
        public ICollection<Partidos.AsignacionCandidatoPuesto> AsignacionesCandidatos { get; set; } = new List<Partidos.AsignacionCandidatoPuesto>();
    }
}
