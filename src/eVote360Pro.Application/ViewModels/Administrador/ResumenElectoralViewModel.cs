namespace eVote360Pro.Core.Application.ViewModels.Administrador
{
    public class ResumenElectoralViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public int CantidadPartidos { get; set; }
        public int CantidadCandidatos { get; set; }
        public int CantidadCiudadanosVotaron { get; set; }
    }
}
