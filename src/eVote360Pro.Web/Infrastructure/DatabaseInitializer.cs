using eVote360Pro.Core.Application.Helpers;
using eVote360Pro.Core.Domain.Entities.Usuarios;
using eVote360Pro.Core.Domain.Enums;
using eVote360Pro.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Web.Infrastructure
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<eVote360ProContext>();

            if (configuration.GetValue("Database:ApplyMigrationsOnStartup", true))
            {
                await context.Database.MigrateAsync();
            }

            if (await context.Usuarios.AnyAsync(u => u.Rol == RolUsuario.Administrador))
                return;

            var section = configuration.GetSection("SeedAdmin");
            var admin = new Usuario
            {
                Nombre = section.GetValue("Nombre", "Administrador"),
                Apellido = section.GetValue("Apellido", "Sistema"),
                CorreoElectronico = section.GetValue("CorreoElectronico", "admin@evote360.local"),
                NombreUsuario = section.GetValue("NombreUsuario", "admin"),
                Contrasena = PasswordEncryptation.HashPassword(section.GetValue("Contrasena", "Admin123*")),
                Rol = RolUsuario.Administrador,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await context.Usuarios.AddAsync(admin);
            await context.SaveChangesAsync();
        }
    }
}
