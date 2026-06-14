namespace eVote360Pro.Core.Application.Dtos.Partidos
{
    public class SaveCandidatoDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public string? Foto { get; set; }
        public required int PartidoPoliticoId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
