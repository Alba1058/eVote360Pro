using eVote360Pro.Core.Domain.Entities.Elecciones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Persistence.EntityConfigurations.Elecciones
{
    public class DetalleVotoConfiguration : IEntityTypeConfiguration<DetalleVoto>
    {
        public void Configure(EntityTypeBuilder<DetalleVoto> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("DetalleVotos");

            builder.Property(x => x.VotoNulo).IsRequired();

            builder.HasOne(x => x.Voto)
                .WithMany(v => v.DetallesVoto)
                .HasForeignKey(x => x.VotoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Candidato)
                .WithMany()
                .HasForeignKey(x => x.CandidatoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PuestoElectivo)
                .WithMany()
                .HasForeignKey(x => x.PuestoElectivoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
