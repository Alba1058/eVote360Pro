namespace eVote360Pro.Core.Application.Dtos.Partidos
{
    public class PartidoPoliticoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Siglas { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
