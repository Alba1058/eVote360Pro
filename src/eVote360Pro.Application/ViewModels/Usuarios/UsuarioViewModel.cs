using eVote360Pro.Core.Domain.Enums;

namespace eVote360Pro.Core.Application.ViewModels.Usuarios
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public RolUsuario Rol { get; set; }
        public int? PartidoPoliticoId { get; set; }
        public string? NombrePartido { get; set; }
        public string? PartidoPoliticoNombre { get; set; }
        public bool IsActive { get; set; }
    }
}
