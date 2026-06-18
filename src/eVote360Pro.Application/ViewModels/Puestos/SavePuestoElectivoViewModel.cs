using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Puestos
{
    public class SavePuestoElectivoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del puesto es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerida")]
        public string Descripcion { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
