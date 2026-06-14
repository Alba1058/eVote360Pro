namespace eVote360Pro.Core.Application.ViewModels.Partidos
{
    public class CandidatoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Foto { get; set; }
        public int PartidoPoliticoId { get; set; }
        public string? NombrePartido { get; set; }
        public bool IsActive { get; set; }
    }
}
