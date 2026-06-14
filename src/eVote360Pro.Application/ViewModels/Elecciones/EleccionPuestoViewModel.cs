namespace eVote360Pro.Core.Application.ViewModels.Elecciones
{
    public class EleccionPuestoViewModel
    {
        public int Id { get; set; }
        public int EleccionId { get; set; }
        public string NombreEleccion { get; set; } = string.Empty;
        public int PuestoElectivoId { get; set; }
        public string NombrePuesto { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
