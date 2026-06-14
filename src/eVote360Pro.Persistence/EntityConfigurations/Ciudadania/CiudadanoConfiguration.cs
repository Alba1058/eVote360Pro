using eVote360Pro.Core.Domain.Entities.Ciudadania;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Persistence.EntityConfigurations.Ciudadania
{
    public class CiudadanoConfiguration : IEntityTypeConfiguration<Ciudadano>
    {
        public void Configure(EntityTypeBuilder<Ciudadano> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("Ciudadanos");

            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Apellido).IsRequired().HasMaxLength(100);
            builder.Property(x => x.CorreoElectronico).IsRequired().HasMaxLength(150);
            builder.Property(x => x.NumeroDocumento).IsRequired().HasMaxLength(20);

            builder.HasIndex(x => x.NumeroDocumento).IsUnique();
        }
    }
}
