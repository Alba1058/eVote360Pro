using eVote360Pro.Core.Domain.Entities.Partidos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Persistence.EntityConfigurations.Partidos
{
    public class AsignacionCandidatoPuestoConfiguration : IEntityTypeConfiguration<AsignacionCandidatoPuesto>
    {
        public void Configure(EntityTypeBuilder<AsignacionCandidatoPuesto> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("AsignacionesCandidatoPuestos");

            builder.Property(x => x.TipoCandidatura).IsRequired();

            builder.HasOne(x => x.Candidato)
                .WithMany(c => c.AsignacionesPuestos)
                .HasForeignKey(x => x.CandidatoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.PuestoElectivo)
                .WithMany(p => p.AsignacionesCandidatos)
                .HasForeignKey(x => x.PuestoElectivoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PartidoPolitico)
                .WithMany(p => p.AsignacionesCandidatos)
                .HasForeignKey(x => x.PartidoPoliticoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
