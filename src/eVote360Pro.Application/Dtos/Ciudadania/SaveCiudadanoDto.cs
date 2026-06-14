namespace eVote360Pro.Core.Application.Dtos.Ciudadania
{
    public class SaveCiudadanoDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string CorreoElectronico { get; set; }
        public required string NumeroDocumento { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
