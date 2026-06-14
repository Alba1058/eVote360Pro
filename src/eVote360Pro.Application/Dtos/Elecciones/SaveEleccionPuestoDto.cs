namespace eVote360Pro.Core.Application.Dtos.Elecciones
{
    public class SaveEleccionPuestoDto
    {
        public int Id { get; set; }
        public required int EleccionId { get; set; }
        public required int PuestoElectivoId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
