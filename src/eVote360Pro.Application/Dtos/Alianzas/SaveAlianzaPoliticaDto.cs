namespace eVote360Pro.Core.Application.Dtos.Alianzas
{
    public class SaveAlianzaPoliticaDto
    {
        public int Id { get; set; }
        public required int Partido1Id { get; set; }
        public required int Partido2Id { get; set; }
        public required int SolicitudAlianzaId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
