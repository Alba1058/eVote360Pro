using eVote360Pro.Core.Domain.Enums;

namespace eVote360Pro.Core.Application.ViewModels.Elecciones
{
    public class EleccionViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public EstadoEleccion Estado { get; set; }
        public bool IsActive { get; set; }
    }
}
