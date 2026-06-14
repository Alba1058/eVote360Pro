using eVote360Pro.Core.Domain.Entities.Puestos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Persistence.EntityConfigurations.Puestos
{
    public class PuestoElectivoConfiguration : IEntityTypeConfiguration<PuestoElectivo>
    {
        public void Configure(EntityTypeBuilder<PuestoElectivo> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("PuestosElectivos");

            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Descripcion).HasMaxLength(500);
        }
    }
}
