using eVote360Pro.Core.Application.Dtos.Usuarios;

namespace eVote360Pro.Core.Application.Interfaces.Usuarios
{
    public interface IUsuarioService
    {
        Task<UsuarioDto?> AddAsync(SaveUsuarioDto dto);
        Task<UsuarioDto?> UpdateAsync(SaveUsuarioDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<UsuarioDto>> GetAll();
        Task<List<UsuarioDto>> GetAllAsync() => GetAll();
        Task<UsuarioDto?> GetById(int id);
        Task<UsuarioDto?> GetByIdAsync(int id) => GetById(id);
        Task<UsuarioDto?> LoginAsync(LoginDto dto);
        Task<UsuarioDto?> GetByNombreUsuarioAsync(string nombreUsuario);
        Task<bool> ExistsByNombreUsuarioAsync(string nombreUsuario, int? excludeId = null);
        Task<List<UsuarioDto>> GetUsuariosSinPartidoAsync();
        Task<List<UsuarioDto>> GetDirigentesConPartidoAsync();
        Task<bool> AssignPartidoAsync(int usuarioId, int partidoPoliticoId);
        Task<bool> RemovePartidoAsync(int usuarioId);
        Task<bool> ActivateAsync(int id);
        Task<bool> DeactivateAsync(int id);
        Task<int> CountActiveAdminsAsync();
    }
}
