using System.ComponentModel.DataAnnotations;
using eVote360Pro.Core.Domain.Enums;

namespace eVote360Pro.Core.Application.ViewModels.Elecciones
{
    public class SaveEleccionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la elección es requerido")]
        [DataType(DataType.Text)]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        [DataType(DataType.DateTime)]
        public required DateTime Fecha { get; set; }

        public EstadoEleccion Estado { get; set; } = EstadoEleccion.Pendiente;
        public bool IsActive { get; set; } = true;
    }
}
