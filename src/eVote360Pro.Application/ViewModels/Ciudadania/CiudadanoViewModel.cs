namespace eVote360Pro.Core.Application.ViewModels.Ciudadania
{
    public class CiudadanoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
