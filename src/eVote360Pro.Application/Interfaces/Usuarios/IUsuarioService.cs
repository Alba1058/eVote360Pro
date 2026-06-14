using eVote360Pro.Core.Application.Dtos.Usuarios;

namespace eVote360Pro.Core.Application.Interfaces.Usuarios
{
    public interface IUsuarioService
    {
        Task<UsuarioDto?> AddAsync(SaveUsuarioDto dto);
        Task<UsuarioDto?> UpdateAsync(SaveUsuarioDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<UsuarioDto>> GetAll();
        Task<UsuarioDto?> GetById(int id);
        Task<UsuarioDto?> LoginAsync(LoginDto dto);
        Task<UsuarioDto?> GetByNombreUsuarioAsync(string nombreUsuario);
    }
}
