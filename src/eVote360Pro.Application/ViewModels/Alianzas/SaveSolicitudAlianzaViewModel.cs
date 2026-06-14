using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Alianzas
{
    public class SaveSolicitudAlianzaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El partido solicitante es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un partido político válido")]
        public required int PartidoSolicitanteId { get; set; }

        [Required(ErrorMessage = "El partido receptor es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un partido político válido")]
        public required int PartidoReceptorId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
