using eVote360Pro.Core.Domain.Entities.Elecciones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Persistence.EntityConfigurations.Elecciones
{
    public class VotoConfiguration : IEntityTypeConfiguration<Voto>
    {
        public void Configure(EntityTypeBuilder<Voto> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("Votos");

            builder.Property(x => x.FechaVoto).IsRequired();

            builder.HasOne(x => x.Ciudadano)
                .WithMany(c => c.Votos)
                .HasForeignKey(x => x.CiudadanoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Eleccion)
                .WithMany(e => e.Votos)
                .HasForeignKey(x => x.EleccionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
