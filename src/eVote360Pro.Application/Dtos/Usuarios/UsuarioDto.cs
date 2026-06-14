using eVote360Pro.Core.Domain.Enums;

namespace eVote360Pro.Core.Application.Dtos.Usuarios
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public RolUsuario Rol { get; set; }
        public int? PartidoPoliticoId { get; set; }
        public string? NombrePartido { get; set; }
        public bool IsActive { get; set; }
    }
}
