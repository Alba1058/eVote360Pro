using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Partidos;
using eVote360Pro.Core.Application.Interfaces.Partidos;
using eVote360Pro.Core.Domain.Entities.Partidos;
using eVote360Pro.Core.Domain.Enums;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Partidos;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Usuarios;

namespace eVote360Pro.Core.Application.Services.Partidos
{
    public class PartidoPoliticoService : IPartidoPoliticoService
    {
        private readonly IPartidoPoliticoRepository _partidoPoliticoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public PartidoPoliticoService(
            IPartidoPoliticoRepository partidoPoliticoRepository,
            IUsuarioRepository usuarioRepository,
            IMapper mapper)
        {
            _partidoPoliticoRepository = partidoPoliticoRepository;
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<PartidoPoliticoDto?> AddAsync(SavePartidoPoliticoDto dto)
        {
            try
            {
                PartidoPolitico entity = _mapper.Map<PartidoPolitico>(dto);
                entity.Siglas = dto.Siglas.Trim().ToUpperInvariant();
                entity.IsActive = dto.IsActive;
                var returnEntity = await _partidoPoliticoRepository.AddAsync(entity);
                return returnEntity == null ? null : _mapper.Map<PartidoPoliticoDto>(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<PartidoPoliticoDto?> UpdateAsync(SavePartidoPoliticoDto dto)
        {
            try
            {
                var existing = await _partidoPoliticoRepository.GetByIdAsync(dto.Id);
                if (existing == null) return null;

                var participated = await HasParticipatedInElectionAsync(dto.Id);
                if (participated)
                {
                    existing.IsActive = dto.IsActive;
                    existing.Descripcion = dto.Descripcion;
                }
                else
                {
                    existing.Nombre = dto.Nombre;
                    existing.Siglas = dto.Siglas.Trim().ToUpperInvariant();
                    existing.Descripcion = dto.Descripcion;
                    if (!string.IsNullOrWhiteSpace(dto.Logo))
                        existing.Logo = dto.Logo;
                    existing.IsActive = dto.IsActive;
                }

                existing.UpdatedAt = DateTime.UtcNow;

                var returnEntity = await _partidoPoliticoRepository.UpdateAsync(existing.Id, existing);
                return returnEntity == null ? null : _mapper.Map<PartidoPoliticoDto>(returnEntity);
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
                await _partidoPoliticoRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<PartidoPoliticoDto?> GetById(int id)
        {
            var entity = await _partidoPoliticoRepository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<PartidoPoliticoDto>(entity);
        }

        public async Task<List<PartidoPoliticoDto>> GetAll()
        {
            var listEntities = await _partidoPoliticoRepository.GetAllAsync();
            return _mapper.Map<List<PartidoPoliticoDto>>(listEntities);
        }

        public async Task<List<PartidoPoliticoDto>> GetAllActiveAsync()
        {
            var all = await GetAll();
            return all.Where(p => p.IsActive).ToList();
        }

        public async Task<PartidoPoliticoDto?> GetBySiglasAsync(string siglas)
        {
            var entity = await _partidoPoliticoRepository.GetBySiglasAsync(siglas.Trim().ToUpperInvariant());
            return entity == null ? null : _mapper.Map<PartidoPoliticoDto>(entity);
        }

        public async Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null)
        {
            var all = await _partidoPoliticoRepository.GetAllAsync();
            return all.Any(p => p.Nombre.Trim().Equals(nombre.Trim(), StringComparison.OrdinalIgnoreCase)
                && (excludeId == null || p.Id != excludeId));
        }

        public async Task<bool> ExistsBySiglasAsync(string siglas, int? excludeId = null)
        {
            var entity = await _partidoPoliticoRepository.GetBySiglasAsync(siglas.Trim().ToUpperInvariant());
            if (entity == null) return false;
            return excludeId == null || entity.Id != excludeId;
        }

        public async Task<bool> HasActiveCandidatesAsync(int partidoPoliticoId)
        {
            return await _partidoPoliticoRepository.HasActiveCandidatesAsync(partidoPoliticoId);
        }

        public async Task<bool> HasParticipatedInElectionAsync(int partidoPoliticoId)
        {
            return await _partidoPoliticoRepository.HasParticipatedInElectionAsync(partidoPoliticoId);
        }

        public async Task<bool> HasActiveLeadersAsync(int partidoPoliticoId)
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return usuarios.Any(u => u.PartidoPoliticoId == partidoPoliticoId && u.IsActive && u.Rol == RolUsuario.DirigentePolitico);
        }

        public async Task<bool> ActivateAsync(int id)
        {
            var entity = await _partidoPoliticoRepository.GetByIdAsync(id);
            if (entity == null || entity.IsActive) return false;
            entity.IsActive = true;
            entity.UpdatedAt = DateTime.UtcNow;
            await _partidoPoliticoRepository.UpdateAsync(id, entity);
            return true;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var entity = await _partidoPoliticoRepository.GetByIdAsync(id);
            if (entity == null || !entity.IsActive) return false;
            entity.IsActive = false;
            entity.UpdatedAt = DateTime.UtcNow;
            await _partidoPoliticoRepository.UpdateAsync(id, entity);
            return true;
        }
    }
}
