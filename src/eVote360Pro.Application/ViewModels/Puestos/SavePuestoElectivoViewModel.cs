using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Puestos
{
    public class SavePuestoElectivoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del puesto es requerido")]
        [DataType(DataType.Text)]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [DataType(DataType.Text)]
        public required string Descripcion { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
