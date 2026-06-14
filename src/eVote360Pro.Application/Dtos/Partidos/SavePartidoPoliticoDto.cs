namespace eVote360Pro.Core.Application.Dtos.Partidos
{
    public class SavePartidoPoliticoDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Siglas { get; set; }
        public required string Logo { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
