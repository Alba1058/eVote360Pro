using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Partidos
{
    public class SavePartidoPoliticoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del partido es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Las siglas son requeridas")]
        public string Siglas { get; set; } = string.Empty;

        public string Logo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerida")]
        public string Descripcion { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public bool BloquearDatosPrincipales { get; set; }
    }
}
