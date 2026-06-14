using eVote360Pro.Core.Domain.Entities.Usuarios;

namespace eVote360Pro.Core.Domain.Interfaces.Repositories.Usuarios
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario);
    }
}
