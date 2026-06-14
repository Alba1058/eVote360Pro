using eVote360Pro.Core.Domain.Entities.Ciudadania;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Persistence.EntityConfigurations.Ciudadania
{
    public class CodigoVerificacionConfiguration : IEntityTypeConfiguration<CodigoVerificacion>
    {
        public void Configure(EntityTypeBuilder<CodigoVerificacion> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("CodigosVerificacion");

            builder.Property(x => x.Codigo).IsRequired().HasMaxLength(10);
            builder.Property(x => x.FechaGeneracion).IsRequired();
            builder.Property(x => x.FechaExpiracion).IsRequired();
            builder.Property(x => x.Usado).IsRequired();

            builder.HasOne(x => x.Ciudadano)
                .WithMany(c => c.CodigosVerificacion)
                .HasForeignKey(x => x.CiudadanoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Eleccion)
                .WithMany()
                .HasForeignKey(x => x.EleccionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
