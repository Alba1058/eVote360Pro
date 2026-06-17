using System.ComponentModel.DataAnnotations;
using eVote360Pro.Core.Domain.Enums;

namespace eVote360Pro.Core.Application.ViewModels.Elecciones
{
    public class SaveEleccionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la elección es requerido")]
        [DataType(DataType.Text)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha es requerida")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Today;

        public EstadoEleccion Estado { get; set; } = EstadoEleccion.Pendiente;
        public bool IsActive { get; set; } = true;
    }
}
