using eVote360Pro.Core.Domain.Enums;

namespace eVote360Pro.Core.Application.Dtos.Elecciones
{
    public class SaveEleccionDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required DateTime Fecha { get; set; }
        public EstadoEleccion Estado { get; set; } = EstadoEleccion.Pendiente;
        public bool IsActive { get; set; } = true;
    }
}
