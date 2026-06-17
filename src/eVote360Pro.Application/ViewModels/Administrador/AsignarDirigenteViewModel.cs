using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Administrador
{
    public class AsignarDirigenteViewModel
    {
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un partido político")]
        public int PartidoPoliticoId { get; set; }
    }
}
