namespace eVote360Pro.Core.Application.ViewModels.Partidos
{
    public class PartidoPoliticoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Siglas { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
