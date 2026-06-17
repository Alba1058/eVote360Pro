namespace eVote360Pro.Core.Application.ViewModels.Alianzas
{
    public class AlianzasIndexViewModel
    {
        public List<SolicitudAlianzaViewModel> SolicitudesPendientes { get; set; } = [];
        public List<SolicitudAlianzaViewModel> SolicitudesEnviadas { get; set; } = [];
        public List<AlianzaPoliticaViewModel> AlianzasVigentes { get; set; } = [];
        public bool HayEleccionActiva { get; set; }
    }
}
