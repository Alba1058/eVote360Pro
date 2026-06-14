using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Elecciones
{
    public class SaveEleccionPuestoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La elección es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una elección válida")]
        public required int EleccionId { get; set; }

        [Required(ErrorMessage = "El puesto electivo es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un puesto electivo válido")]
        public required int PuestoElectivoId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
