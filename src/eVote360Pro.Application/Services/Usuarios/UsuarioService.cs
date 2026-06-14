using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Usuarios;
using eVote360Pro.Core.Application.Interfaces.Usuarios;
using eVote360Pro.Core.Domain.Entities.Usuarios;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Usuarios;

namespace eVote360Pro.Core.Application.Services.Usuarios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<UsuarioDto?> AddAsync(SaveUsuarioDto dto)
        {
            try
            {
                Usuario entity = _mapper.Map<Usuario>(dto);
                Usuario? returnEntity = await _usuarioRepository.AddAsync(entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<UsuarioDto>(returnEntity);
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
                Usuario entity = _mapper.Map<Usuario>(dto);
                Usuario? returnEntity = await _usuarioRepository.UpdateAsync(entity.Id, entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<UsuarioDto>(returnEntity);
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
            try
            {
                var entity = await _usuarioRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<UsuarioDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<UsuarioDto>> GetAll()
        {
            try
            {
                var listEntities = await _usuarioRepository.GetAllAsync();
                return _mapper.Map<List<UsuarioDto>>(listEntities);
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<UsuarioDto?> LoginAsync(LoginDto dto)
        {
            try
            {
                var entity = await _usuarioRepository.GetByNombreUsuarioAsync(dto.NombreUsuario);
                if (entity == null)
                {
                    return null;
                }

                if (entity.Contrasena != dto.Contrasena)
                {
                    return null;
                }

                return _mapper.Map<UsuarioDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<UsuarioDto?> GetByNombreUsuarioAsync(string nombreUsuario)
        {
            try
            {
                var entity = await _usuarioRepository.GetByNombreUsuarioAsync(nombreUsuario);
                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<UsuarioDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
