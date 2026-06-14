using eVote360Pro.Core.Domain.Entities.Partidos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Persistence.EntityConfigurations.Partidos
{
    public class CandidatoConfiguration : IEntityTypeConfiguration<Candidato>
    {
        public void Configure(EntityTypeBuilder<Candidato> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("Candidatos");

            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Apellido).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Foto).HasMaxLength(500);

            builder.HasOne(x => x.PartidoPolitico)
                .WithMany(p => p.Candidatos)
                .HasForeignKey(x => x.PartidoPoliticoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
