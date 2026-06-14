using System.ComponentModel.DataAnnotations;
using eVote360Pro.Core.Domain.Enums;

namespace eVote360Pro.Core.Application.ViewModels.Partidos
{
    public class SaveAsignacionCandidatoPuestoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El candidato es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un candidato válido")]
        public required int CandidatoId { get; set; }

        [Required(ErrorMessage = "El puesto electivo es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un puesto electivo válido")]
        public required int PuestoElectivoId { get; set; }

        [Required(ErrorMessage = "El partido político es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un partido político válido")]
        public required int PartidoPoliticoId { get; set; }

        [Required(ErrorMessage = "El tipo de candidatura es requerido")]
        public required TipoCandidatura TipoCandidatura { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
