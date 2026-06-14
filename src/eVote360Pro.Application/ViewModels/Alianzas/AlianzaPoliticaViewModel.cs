namespace eVote360Pro.Core.Application.ViewModels.Alianzas
{
    public class AlianzaPoliticaViewModel
    {
        public int Id { get; set; }
        public DateTime FechaAceptacion { get; set; }
        public int Partido1Id { get; set; }
        public string NombrePartido1 { get; set; } = string.Empty;
        public string SiglasPartido1 { get; set; } = string.Empty;
        public int Partido2Id { get; set; }
        public string NombrePartido2 { get; set; } = string.Empty;
        public string SiglasPartido2 { get; set; } = string.Empty;
        public int SolicitudAlianzaId { get; set; }
        public bool IsActive { get; set; }
    }
}
