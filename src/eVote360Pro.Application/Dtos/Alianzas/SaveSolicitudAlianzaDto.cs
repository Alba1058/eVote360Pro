namespace eVote360Pro.Core.Application.Dtos.Alianzas
{
    public class SaveSolicitudAlianzaDto
    {
        public int Id { get; set; }
        public required int PartidoSolicitanteId { get; set; }
        public required int PartidoReceptorId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
