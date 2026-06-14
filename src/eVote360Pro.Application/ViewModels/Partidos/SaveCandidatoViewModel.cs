using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Partidos
{
    public class SaveCandidatoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [DataType(DataType.Text)]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")]
        [DataType(DataType.Text)]
        public required string Apellido { get; set; }

        [DataType(DataType.Text)]
        public string? Foto { get; set; }

        [Required(ErrorMessage = "El partido político es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un partido político válido")]
        public required int PartidoPoliticoId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
