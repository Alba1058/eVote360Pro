using System.ComponentModel.DataAnnotations;
using eVote360Pro.Core.Domain.Enums;

namespace eVote360Pro.Core.Application.ViewModels.Partidos
{
    public class SaveAsignacionCandidatoPuestoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El candidato es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un candidato válido")]
        public int CandidatoId { get; set; }

        [Required(ErrorMessage = "El puesto electivo es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un puesto electivo válido")]
        public int PuestoElectivoId { get; set; }

        [Required(ErrorMessage = "El partido político es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un partido político válido")]
        public int PartidoPoliticoId { get; set; }

        [Required(ErrorMessage = "El tipo de candidatura es requerido")]
        public TipoCandidatura TipoCandidatura { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
