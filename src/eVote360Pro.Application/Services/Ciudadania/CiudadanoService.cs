using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Ciudadania;
using eVote360Pro.Core.Application.Interfaces.Ciudadania;
using eVote360Pro.Core.Domain.Entities.Ciudadania;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Ciudadania;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Elecciones;

namespace eVote360Pro.Core.Application.Services.Ciudadania
{
    public class CiudadanoService : ICiudadanoService
    {
        private readonly ICiudadanoRepository _ciudadanoRepository;
        private readonly IVotoRepository _votoRepository;
        private readonly IMapper _mapper;

        public CiudadanoService(
            ICiudadanoRepository ciudadanoRepository,
            IVotoRepository votoRepository,
            IMapper mapper)
        {
            _ciudadanoRepository = ciudadanoRepository;
            _votoRepository = votoRepository;
            _mapper = mapper;
        }

        public async Task<CiudadanoDto?> AddAsync(SaveCiudadanoDto dto)
        {
            try
            {
                Ciudadano entity = _mapper.Map<Ciudadano>(dto);
                entity.NumeroDocumento = dto.NumeroDocumento.Trim();
                entity.IsActive = dto.IsActive;
                Ciudadano? returnEntity = await _ciudadanoRepository.AddAsync(entity);
                return returnEntity == null ? null : _mapper.Map<CiudadanoDto>(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<CiudadanoDto?> UpdateAsync(SaveCiudadanoDto dto)
        {
            try
            {
                var existing = await _ciudadanoRepository.GetByIdAsync(dto.Id);
                if (existing == null) return null;

                var participated = await HasParticipatedInElectionAsync(dto.Id);
                existing.Nombre = dto.Nombre;
                existing.Apellido = dto.Apellido;
                existing.CorreoElectronico = dto.CorreoElectronico;
                existing.IsActive = dto.IsActive;
                if (!participated)
                {
                    existing.NumeroDocumento = dto.NumeroDocumento.Trim();
                }

                existing.UpdatedAt = DateTime.UtcNow;
                var returnEntity = await _ciudadanoRepository.UpdateAsync(existing.Id, existing);
                return returnEntity == null ? null : _mapper.Map<CiudadanoDto>(returnEntity);
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
                await _ciudadanoRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<CiudadanoDto?> GetById(int id)
        {
            try
            {
                var entity = await _ciudadanoRepository.GetByIdAsync(id);
                return entity == null ? null : _mapper.Map<CiudadanoDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<CiudadanoDto>> GetAll()
        {
            try
            {
                var listEntities = await _ciudadanoRepository.GetAllAsync();
                return _mapper.Map<List<CiudadanoDto>>(listEntities);
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<CiudadanoDto?> GetByNumeroDocumentoAsync(string numeroDocumento)
        {
            try
            {
                var entity = await _ciudadanoRepository.GetByNumeroDocumentoAsync(numeroDocumento.Trim());
                return entity == null ? null : _mapper.Map<CiudadanoDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> ExistsByNumeroDocumentoAsync(string numeroDocumento, int? excludeId = null)
        {
            var entity = await _ciudadanoRepository.GetByNumeroDocumentoAsync(numeroDocumento.Trim());
            if (entity == null) return false;
            return excludeId == null || entity.Id != excludeId;
        }

        public async Task<bool> ExistsByCorreoAsync(string correo, int? excludeId = null)
        {
            var all = await _ciudadanoRepository.GetAllAsync();
            return all.Any(c => c.CorreoElectronico.Equals(correo, StringComparison.OrdinalIgnoreCase)
                && (excludeId == null || c.Id != excludeId));
        }

        public async Task<bool> ActivateAsync(int id)
        {
            var entity = await _ciudadanoRepository.GetByIdAsync(id);
            if (entity == null || entity.IsActive) return false;
            entity.IsActive = true;
            entity.UpdatedAt = DateTime.UtcNow;
            await _ciudadanoRepository.UpdateAsync(id, entity);
            return true;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var entity = await _ciudadanoRepository.GetByIdAsync(id);
            if (entity == null || !entity.IsActive) return false;
            entity.IsActive = false;
            entity.UpdatedAt = DateTime.UtcNow;
            await _ciudadanoRepository.UpdateAsync(id, entity);
            return true;
        }

        public async Task<bool> HasVotedInElectionAsync(int ciudadanoId, int eleccionId)
        {
            return await _ciudadanoRepository.HasVotedInElectionAsync(ciudadanoId, eleccionId);
        }

        public async Task<bool> MarkAsVotedAsync(int ciudadanoId, int eleccionId)
        {
            return await _ciudadanoRepository.HasVotedInElectionAsync(ciudadanoId, eleccionId);
        }

        public async Task<bool> HasParticipatedInElectionAsync(int ciudadanoId)
        {
            var votos = await _votoRepository.GetAllAsync();
            return votos.Any(v => v.CiudadanoId == ciudadanoId);
        }
    }
}
