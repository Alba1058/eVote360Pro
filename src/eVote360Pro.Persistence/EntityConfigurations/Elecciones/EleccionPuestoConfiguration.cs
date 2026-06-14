using eVote360Pro.Core.Domain.Entities.Elecciones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Persistence.EntityConfigurations.Elecciones
{
    public class EleccionPuestoConfiguration : IEntityTypeConfiguration<EleccionPuesto>
    {
        public void Configure(EntityTypeBuilder<EleccionPuesto> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("EleccionesPuestos");

            builder.HasOne(x => x.Eleccion)
                .WithMany(e => e.EleccionesPuestos)
                .HasForeignKey(x => x.EleccionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.PuestoElectivo)
                .WithMany(p => p.EleccionesPuestos)
                .HasForeignKey(x => x.PuestoElectivoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
