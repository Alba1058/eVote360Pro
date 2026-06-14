namespace eVote360Pro.Core.Application.ViewModels.Puestos
{
    public class PuestoElectivoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
