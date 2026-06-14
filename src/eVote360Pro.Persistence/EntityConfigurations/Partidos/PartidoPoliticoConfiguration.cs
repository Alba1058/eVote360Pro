using eVote360Pro.Core.Domain.Entities.Partidos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Persistence.EntityConfigurations.Partidos
{
    public class PartidoPoliticoConfiguration : IEntityTypeConfiguration<PartidoPolitico>
    {
        public void Configure(EntityTypeBuilder<PartidoPolitico> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("PartidosPoliticos");

            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Siglas).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Logo).HasMaxLength(500);

            builder.HasIndex(x => x.Siglas).IsUnique();
        }
    }
}
