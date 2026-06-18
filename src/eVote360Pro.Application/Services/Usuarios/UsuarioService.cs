using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Usuarios;
using eVote360Pro.Core.Application.Helpers;
using eVote360Pro.Core.Application.Interfaces.Usuarios;
using eVote360Pro.Core.Domain.Entities.Usuarios;
using eVote360Pro.Core.Domain.Enums;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Partidos;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Usuarios;

namespace eVote360Pro.Core.Application.Services.Usuarios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPartidoPoliticoRepository _partidoPoliticoRepository;
        private readonly IMapper _mapper;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IPartidoPoliticoRepository partidoPoliticoRepository,
            IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _partidoPoliticoRepository = partidoPoliticoRepository;
            _mapper = mapper;
        }

        public async Task<UsuarioDto?> AddAsync(SaveUsuarioDto dto)
        {
            try
            {
                Usuario entity = _mapper.Map<Usuario>(dto);
                entity.NombreUsuario = dto.NombreUsuario.Trim();
                entity.Contrasena = PasswordEncryptation.HashPassword(dto.Contrasena);
                entity.IsActive = dto.IsActive;
                var returnEntity = await _usuarioRepository.AddAsync(entity);
                return returnEntity == null ? null : await MapUsuarioDtoAsync(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<UsuarioDto?> UpdateAsync(SaveUsuarioDto dto)
        {
            try
            {
                var existing = await _usuarioRepository.GetByIdAsync(dto.Id);
                if (existing == null) return null;

                existing.NombreUsuario = dto.NombreUsuario.Trim();
                existing.Rol = dto.Rol;
                existing.PartidoPoliticoId = dto.PartidoPoliticoId;
                existing.IsActive = dto.IsActive;

                if (!string.IsNullOrWhiteSpace(dto.Contrasena))
                {
                    existing.Contrasena = PasswordEncryptation.HashPassword(dto.Contrasena);
                }

                existing.UpdatedAt = DateTime.UtcNow;
                var returnEntity = await _usuarioRepository.UpdateAsync(existing.Id, existing);
                return returnEntity == null ? null : await MapUsuarioDtoAsync(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _usuarioRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<UsuarioDto?> GetById(int id)
        {
            var entity = await _usuarioRepository.GetByIdAsync(id);
            return entity == null ? null : await MapUsuarioDtoAsync(entity);
        }

        public async Task<List<UsuarioDto>> GetAll()
        {
            var listEntities = await _usuarioRepository.GetAllAsync();
            var result = new List<UsuarioDto>();
            foreach (var entity in listEntities)
            {
                var dto = await MapUsuarioDtoAsync(entity);
                if (dto != null) result.Add(dto);
            }
            return result;
        }

        public async Task<UsuarioDto?> LoginAsync(LoginDto dto)
        {
            var entity = await _usuarioRepository.GetByNombreUsuarioAsync(dto.NombreUsuario.Trim());
            if (entity == null) return null;

            var passwordIsValid = PasswordEncryptation.VerifyPassword(dto.Contrasena, entity.Contrasena);
            if (!passwordIsValid && PasswordEncryptation.IsSha256Hash(entity.Contrasena))
            {
                passwordIsValid = entity.Contrasena == PasswordEncryptation.ComputeSha256Hash(dto.Contrasena);
                if (passwordIsValid)
                {
                    entity.Contrasena = PasswordEncryptation.HashPassword(dto.Contrasena);
                    entity.UpdatedAt = DateTime.UtcNow;
                    await _usuarioRepository.UpdateAsync(entity.Id, entity);
                }
            }

            if (!passwordIsValid)
                return null;

            return await MapUsuarioDtoAsync(entity);
        }

        public async Task<UsuarioDto?> GetByNombreUsuarioAsync(string nombreUsuario)
        {
            var entity = await _usuarioRepository.GetByNombreUsuarioAsync(nombreUsuario.Trim());
            return entity == null ? null : await MapUsuarioDtoAsync(entity);
        }

        public async Task<bool> ExistsByNombreUsuarioAsync(string nombreUsuario, int? excludeId = null)
        {
            var entity = await _usuarioRepository.GetByNombreUsuarioAsync(nombreUsuario.Trim());
            if (entity == null) return false;
            return excludeId == null || entity.Id != excludeId;
        }

        public async Task<List<UsuarioDto>> GetUsuariosSinPartidoAsync()
        {
            var all = await _usuarioRepository.GetAllAsync();
            var result = new List<UsuarioDto>();
            foreach (var u in all.Where(u => u.Rol == RolUsuario.DirigentePolitico && u.IsActive && !u.PartidoPoliticoId.HasValue))
            {
                var dto = await MapUsuarioDtoAsync(u);
                if (dto != null) result.Add(dto);
            }
            return result;
        }

        public async Task<List<UsuarioDto>> GetDirigentesConPartidoAsync()
        {
            var all = await _usuarioRepository.GetAllAsync();
            var result = new List<UsuarioDto>();
            foreach (var u in all.Where(u => u.Rol == RolUsuario.DirigentePolitico && u.PartidoPoliticoId.HasValue))
            {
                var dto = await MapUsuarioDtoAsync(u);
                if (dto != null) result.Add(dto);
            }
            return result;
        }

        public async Task<bool> AssignPartidoAsync(int usuarioId, int partidoPoliticoId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null || usuario.Rol != RolUsuario.DirigentePolitico) return false;

            var partido = await _partidoPoliticoRepository.GetByIdAsync(partidoPoliticoId);
            if (partido == null || !partido.IsActive) return false;

            var usuarios = await _usuarioRepository.GetAllAsync();
            if (usuarios.Any(u => u.PartidoPoliticoId == partidoPoliticoId && u.Id != usuarioId))
                return false;

            usuario.PartidoPoliticoId = partidoPoliticoId;
            usuario.UpdatedAt = DateTime.UtcNow;
            await _usuarioRepository.UpdateAsync(usuario.Id, usuario);
            return true;
        }

        public async Task<bool> RemovePartidoAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null || !usuario.PartidoPoliticoId.HasValue) return false;
            usuario.PartidoPoliticoId = null;
            usuario.UpdatedAt = DateTime.UtcNow;
            await _usuarioRepository.UpdateAsync(usuario.Id, usuario);
            return true;
        }

        public async Task<bool> ActivateAsync(int id)
        {
            var entity = await _usuarioRepository.GetByIdAsync(id);
            if (entity == null || entity.IsActive) return false;
            entity.IsActive = true;
            entity.UpdatedAt = DateTime.UtcNow;
            await _usuarioRepository.UpdateAsync(id, entity);
            return true;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var entity = await _usuarioRepository.GetByIdAsync(id);
            if (entity == null || !entity.IsActive) return false;
            entity.IsActive = false;
            entity.UpdatedAt = DateTime.UtcNow;
            await _usuarioRepository.UpdateAsync(id, entity);
            return true;
        }

        public async Task<int> CountActiveAdminsAsync()
        {
            var all = await _usuarioRepository.GetAllAsync();
            return all.Count(u => u.Rol == RolUsuario.Administrador && u.IsActive);
        }

        private async Task<UsuarioDto?> MapUsuarioDtoAsync(Usuario entity)
        {
            var dto = _mapper.Map<UsuarioDto>(entity);
            if (entity.PartidoPoliticoId.HasValue)
            {
                var partido = await _partidoPoliticoRepository.GetByIdAsync(entity.PartidoPoliticoId.Value);
                dto.NombrePartido = partido?.Nombre;
                dto.PartidoPoliticoSiglas = partido?.Siglas;
                dto.PartidoPoliticoActivo = partido?.IsActive ?? false;
            }
            return dto;
        }
    }
}
