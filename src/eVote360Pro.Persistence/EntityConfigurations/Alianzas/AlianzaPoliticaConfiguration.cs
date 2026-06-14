using eVote360Pro.Core.Domain.Entities.Alianzas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Persistence.EntityConfigurations.Alianzas
{
    public class AlianzaPoliticaConfiguration : IEntityTypeConfiguration<AlianzaPolitica>
    {
        public void Configure(EntityTypeBuilder<AlianzaPolitica> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("AlianzasPoliticas");

            builder.Property(x => x.FechaAceptacion).IsRequired();

            builder.HasOne(x => x.Partido1)
                .WithMany(p => p.AlianzasComoPartido1)
                .HasForeignKey(x => x.Partido1Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Partido2)
                .WithMany(p => p.AlianzasComoPartido2)
                .HasForeignKey(x => x.Partido2Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SolicitudAlianza)
                .WithMany()
                .HasForeignKey(x => x.SolicitudAlianzaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
