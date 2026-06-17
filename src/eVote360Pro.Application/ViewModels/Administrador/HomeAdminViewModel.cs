namespace eVote360Pro.Core.Application.ViewModels.Administrador
{
    public class HomeAdminViewModel
    {
        public List<int> AniosDisponibles { get; set; } = new();
        public int AnioSeleccionado { get; set; }
        public List<ResumenElectoralViewModel> Resumenes { get; set; } = new();
    }
}
