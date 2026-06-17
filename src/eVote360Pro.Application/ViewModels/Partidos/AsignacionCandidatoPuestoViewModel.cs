using eVote360Pro.Core.Domain.Enums;

namespace eVote360Pro.Core.Application.ViewModels.Partidos
{
    public class AsignacionCandidatoPuestoViewModel
    {
        public int Id { get; set; }
        public int CandidatoId { get; set; }
        public string NombreCandidato { get; set; } = string.Empty;
        public string ApellidoCandidato { get; set; } = string.Empty;
        public string CandidatoNombre => NombreCandidato;
        public string CandidatoApellido => ApellidoCandidato;
        public int PuestoElectivoId { get; set; }
        public string NombrePuesto { get; set; } = string.Empty;
        public string PuestoElectivoNombre => NombrePuesto;
        public int PartidoPoliticoId { get; set; }
        public string NombrePartido { get; set; } = string.Empty;
        public string PartidoOrigenNombre { get; set; } = string.Empty;
        public TipoCandidatura TipoCandidatura { get; set; }
        public string TipoCandidaturaNombre => TipoCandidatura == TipoCandidatura.Propio ? "Propio" : "Aliado";
        public bool IsActive { get; set; }
    }
}
