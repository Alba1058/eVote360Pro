using System.ComponentModel.DataAnnotations;
using eVote360Pro.Core.Domain.Enums;

namespace eVote360Pro.Core.Application.ViewModels.Alianzas
{
    public class SaveSolicitudAlianzaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El partido político es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un partido político válido")]
        public int PartidoReceptorId { get; set; }

        public int PartidoSolicitanteId { get; set; }

        public int PartidoPoliticoId
        {
            get => PartidoReceptorId;
            set => PartidoReceptorId = value;
        }

        public bool IsActive { get; set; } = true;
    }
}
