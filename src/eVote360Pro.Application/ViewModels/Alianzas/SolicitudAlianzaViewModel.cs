using eVote360Pro.Core.Domain.Enums;

namespace eVote360Pro.Core.Application.ViewModels.Alianzas
{
    public class SolicitudAlianzaViewModel
    {
        public int Id { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public EstadoSolicitudAlianza Estado { get; set; }
        public int PartidoSolicitanteId { get; set; }
        public string NombrePartidoSolicitante { get; set; } = string.Empty;
        public string SiglasPartidoSolicitante { get; set; } = string.Empty;
        public int PartidoReceptorId { get; set; }
        public string NombrePartidoReceptor { get; set; } = string.Empty;
        public string SiglasPartidoReceptor { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
