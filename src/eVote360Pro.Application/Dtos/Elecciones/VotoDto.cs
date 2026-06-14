namespace eVote360Pro.Core.Application.Dtos.Elecciones
{
    public class VotoDto
    {
        public int Id { get; set; }
        public int CiudadanoId { get; set; }
        public string NombreCiudadano { get; set; } = string.Empty;
        public int EleccionId { get; set; }
        public string NombreEleccion { get; set; } = string.Empty;
        public DateTime FechaVoto { get; set; }
        public bool IsActive { get; set; }
    }
}
