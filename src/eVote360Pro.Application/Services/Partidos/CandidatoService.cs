using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Partidos;
using eVote360Pro.Core.Application.Interfaces.Partidos;
using eVote360Pro.Core.Domain.Entities.Partidos;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Partidos;

namespace eVote360Pro.Core.Application.Services.Partidos
{
    public class CandidatoService : ICandidatoService
    {
        private readonly ICandidatoRepository _candidatoRepository;
        private readonly IMapper _mapper;

        public CandidatoService(ICandidatoRepository candidatoRepository, IMapper mapper)
        {
            _candidatoRepository = candidatoRepository;
            _mapper = mapper;
        }

        public async Task<CandidatoDto?> AddAsync(SaveCandidatoDto dto)
        {
            try
            {
                Candidato entity = _mapper.Map<Candidato>(dto);
                Candidato? returnEntity = await _candidatoRepository.AddAsync(entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<CandidatoDto>(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<CandidatoDto?> UpdateAsync(SaveCandidatoDto dto)
        {
            try
            {
                Candidato entity = _mapper.Map<Candidato>(dto);
                Candidato? returnEntity = await _candidatoRepository.UpdateAsync(entity.Id, entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<CandidatoDto>(returnEntity);
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
                await _candidatoRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<CandidatoDto?> GetById(int id)
        {
            try
            {
                var entity = await _candidatoRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<CandidatoDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<CandidatoDto>> GetAll()
        {
            try
            {
                var listEntities = await _candidatoRepository.GetAllAsync();
                return _mapper.Map<List<CandidatoDto>>(listEntities);
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<bool> HasParticipatedInElectionAsync(int candidatoId)
        {
            try
            {
                return await _candidatoRepository.HasParticipatedInElectionAsync(candidatoId);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> IsAssignedToPositionAsync(int candidatoId)
        {
            try
            {
                return await _candidatoRepository.IsAssignedToPositionAsync(candidatoId);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<CandidatoDto>> GetByPartidoPoliticoAsync(int partidoPoliticoId)
        {
            var all = await _candidatoRepository.GetAllAsync();
            return _mapper.Map<List<CandidatoDto>>(all.Where(c => c.PartidoPoliticoId == partidoPoliticoId));
        }

        public async Task<bool> ActivateAsync(int id)
        {
            var entity = await _candidatoRepository.GetByIdAsync(id);
            if (entity == null || entity.IsActive) return false;
            entity.IsActive = true;
            entity.UpdatedAt = DateTime.UtcNow;
            await _candidatoRepository.UpdateAsync(id, entity);
            return true;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var entity = await _candidatoRepository.GetByIdAsync(id);
            if (entity == null || !entity.IsActive) return false;
            entity.IsActive = false;
            entity.UpdatedAt = DateTime.UtcNow;
            await _candidatoRepository.UpdateAsync(id, entity);
            return true;
        }
    }
}
