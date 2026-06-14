namespace eVote360Pro.Core.Application.Dtos.Puestos
{
    public class SavePuestoElectivoDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
