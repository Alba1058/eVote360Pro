namespace eVote360Pro.Core.Application.ViewModels.Administrador
{
    public class UsuarioDirigenteViewModel
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string NombreCompleto => $"{Nombre} {Apellido}";
        public int? PartidoPoliticoId { get; set; }
        public string? PartidoNombre { get; set; }
        public string? PartidoSiglas { get; set; }
        public bool IsActive { get; set; }
        public bool PartidoActivo { get; set; }
    }
}
