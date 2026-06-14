using eVote360Pro.Core.Domain.Entities.Alianzas;
using eVote360Pro.Core.Domain.Entities.Ciudadania;
using eVote360Pro.Core.Domain.Entities.Elecciones;
using eVote360Pro.Core.Domain.Entities.Partidos;
using eVote360Pro.Core.Domain.Entities.Puestos;
using eVote360Pro.Core.Domain.Entities.Usuarios;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace eVote360Pro.Persistence.Contexts
{
    public class eVote360ProContext : DbContext
    {
        public eVote360ProContext(DbContextOptions<eVote360ProContext> options) : base(options)
        {
        }

        // Alianzas
        public DbSet<AlianzaPolitica> AlianzaPoliticas { get; set; }
        public DbSet<SolicitudAlianza> SolicitudAlianzas { get; set; }

        // Ciudadania
        public DbSet<Ciudadano> Ciudadanos { get; set; }
        public DbSet<CodigoVerificacion> CodigoVerificaciones { get; set; }

        // Elecciones
        public DbSet<DetalleVoto> DetalleVotos { get; set; }
        public DbSet<Eleccion> Elecciones { get; set; }
        public DbSet<EleccionPuesto> EleccionPuestos { get; set; }
        public DbSet<Voto> Votos { get; set; }

        // Partidos
        public DbSet<AsignacionCandidatoPuesto> AsignacionCandidatos { get; set; }
        public DbSet<Candidato> Candidatos { get; set; }
        public DbSet<PartidoPolitico> PartidoPoliticos { get; set; }

        // Puestos
        public DbSet<PuestoElectivo> PuestoElectivos { get; set; }

        // Usuarios
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
