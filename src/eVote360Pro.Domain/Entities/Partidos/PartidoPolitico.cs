using eVote360Pro.Core.Domain.Common;

namespace eVote360Pro.Core.Domain.Entities.Partidos
{
    public class PartidoPolitico : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string Siglas { get; set; } = string.Empty;
        public string? Logo { get; set; }

        public ICollection<Usuarios.Usuario> Usuarios { get; set; } = new List<Usuarios.Usuario>();
        public ICollection<Candidato> Candidatos { get; set; } = new List<Candidato>();
        public ICollection<Alianzas.SolicitudAlianza> SolicitudesEnviadas { get; set; } = new List<Alianzas.SolicitudAlianza>();
        public ICollection<Alianzas.SolicitudAlianza> SolicitudesRecibidas { get; set; } = new List<Alianzas.SolicitudAlianza>();
        public ICollection<Alianzas.AlianzaPolitica> AlianzasComoPartido1 { get; set; } = new List<Alianzas.AlianzaPolitica>();
        public ICollection<Alianzas.AlianzaPolitica> AlianzasComoPartido2 { get; set; } = new List<Alianzas.AlianzaPolitica>();
        public ICollection<AsignacionCandidatoPuesto> AsignacionesCandidatos { get; set; } = new List<AsignacionCandidatoPuesto>();
    }
}
