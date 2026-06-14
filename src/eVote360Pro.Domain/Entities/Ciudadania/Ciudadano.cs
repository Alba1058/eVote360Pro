using eVote360Pro.Core.Domain.Common;

namespace eVote360Pro.Core.Domain.Entities.Ciudadania
{
    public class Ciudadano : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<Elecciones.Voto> Votos { get; set; } = new List<Elecciones.Voto>();
        public ICollection<CodigoVerificacion> CodigosVerificacion { get; set; } = new List<CodigoVerificacion>();
    }
}
