using eVote360Pro.Core.Domain.Entities.Alianzas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Persistence.EntityConfigurations.Alianzas
{
    public class SolicitudAlianzaConfiguration : IEntityTypeConfiguration<SolicitudAlianza>
    {
        public void Configure(EntityTypeBuilder<SolicitudAlianza> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("SolicitudesAlianzas");

            builder.Property(x => x.FechaSolicitud).IsRequired();
            builder.Property(x => x.Estado).IsRequired();

            builder.HasOne(x => x.PartidoSolicitante)
                .WithMany(p => p.SolicitudesEnviadas)
                .HasForeignKey(x => x.PartidoSolicitanteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PartidoReceptor)
                .WithMany(p => p.SolicitudesRecibidas)
                .HasForeignKey(x => x.PartidoReceptorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
