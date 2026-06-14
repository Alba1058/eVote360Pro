using eVote360Pro.Core.Domain.Entities.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eVote360Pro.Persistence.EntityConfigurations.Usuarios
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("Usuarios");

            builder.Property(x => x.NombreUsuario).IsRequired().HasMaxLength(100);
            builder.HasIndex(x => x.NombreUsuario).IsUnique();
            builder.Property(x => x.Contrasena).IsRequired().HasMaxLength(256);

            builder.HasOne(x => x.PartidoPolitico)
                .WithMany(p => p.Usuarios)
                .HasForeignKey(x => x.PartidoPoliticoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Rol).IsRequired();
        }
    }
}
