namespace eVote360Pro.Core.Application.ViewModels.Dirigente
{
    public class HomeDirigenteViewModel
    {
        public string PartidoNombre { get; set; } = string.Empty;
        public string PartidoSiglas { get; set; } = string.Empty;
        public string PartidoLogo { get; set; } = string.Empty;
        public int CandidatosActivos { get; set; }
        public int CandidatosInactivos { get; set; }
        public int AlianzasPoliticas { get; set; }
        public int SolicitudesPendientes { get; set; }
        public int CandidatosAsignados { get; set; }
    }
}
