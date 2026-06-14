using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Partidos
{
    public class SavePartidoPoliticoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del partido es requerido")]
        [DataType(DataType.Text)]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "Las siglas son requeridas")]
        [DataType(DataType.Text)]
        public required string Siglas { get; set; }

        [Required(ErrorMessage = "El logo es requerido")]
        [DataType(DataType.Text)]
        public required string Logo { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
