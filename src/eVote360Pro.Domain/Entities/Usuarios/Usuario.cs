using eVote360Pro.Core.Domain.Common;
using eVote360Pro.Core.Domain.Enums;

namespace eVote360Pro.Core.Domain.Entities.Usuarios
{
    public class Usuario : BaseEntity
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public RolUsuario Rol { get; set; }

        // Navigation properties
        public int? PartidoPoliticoId { get; set; }
        public Partidos.PartidoPolitico? PartidoPolitico { get; set; }
    }
}
