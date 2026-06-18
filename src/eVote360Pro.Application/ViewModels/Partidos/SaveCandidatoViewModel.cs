using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Partidos
{
    public class SaveCandidatoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        public string Apellido { get; set; } = string.Empty;

        public string Foto { get; set; } = string.Empty;

        public int PartidoPoliticoId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
