using eVote360Pro.Core.Domain.Enums;

namespace eVote360Pro.Core.Application.Dtos.Usuarios
{
    public class SaveUsuarioDto
    {
        public int Id { get; set; }
        public required string NombreUsuario { get; set; }
        public required string Contrasena { get; set; }
        public required RolUsuario Rol { get; set; }
        public int? PartidoPoliticoId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
