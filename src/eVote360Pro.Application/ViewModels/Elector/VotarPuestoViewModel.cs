using System.ComponentModel.DataAnnotations;

namespace eVote360Pro.Core.Application.ViewModels.Elector
{
    public class VotarPuestoViewModel
    {
        public int PuestoElectivoId { get; set; }
        public string PuestoNombre { get; set; } = string.Empty;
        public List<CandidatoVotacionViewModel> Candidatos { get; set; } = new();

        [Required(ErrorMessage = "Debe seleccionar un candidato")]
        public int? CandidatoSeleccionadoId { get; set; }
    }
}
