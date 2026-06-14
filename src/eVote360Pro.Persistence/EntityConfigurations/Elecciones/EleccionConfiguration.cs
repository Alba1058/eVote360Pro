using eVote360Pro.Core.Domain.Entities.Elecciones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Persistence.EntityConfigurations.Elecciones
{
    public class EleccionConfiguration : IEntityTypeConfiguration<Eleccion>
    {
        public void Configure(EntityTypeBuilder<Eleccion> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("Elecciones");

            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Fecha).IsRequired();
            builder.Property(x => x.Estado).IsRequired();
        }
    }
}
