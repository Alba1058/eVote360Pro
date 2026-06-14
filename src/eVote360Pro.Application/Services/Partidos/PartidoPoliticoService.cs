using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Partidos;
using eVote360Pro.Core.Application.Interfaces.Partidos;
using eVote360Pro.Core.Domain.Entities.Partidos;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Partidos;

namespace eVote360Pro.Core.Application.Services.Partidos
{
    public class PartidoPoliticoService : IPartidoPoliticoService
    {
        private readonly IPartidoPoliticoRepository _partidoPoliticoRepository;
        private readonly IMapper _mapper;

        public PartidoPoliticoService(IPartidoPoliticoRepository partidoPoliticoRepository, IMapper mapper)
        {
            _partidoPoliticoRepository = partidoPoliticoRepository;
            _mapper = mapper;
        }

        public async Task<PartidoPoliticoDto?> AddAsync(SavePartidoPoliticoDto dto)
        {
            try
            {
                PartidoPolitico entity = _mapper.Map<PartidoPolitico>(dto);
                PartidoPolitico? returnEntity = await _partidoPoliticoRepository.AddAsync(entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<PartidoPoliticoDto>(returnEntity);
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
                PartidoPolitico entity = _mapper.Map<PartidoPolitico>(dto);
                PartidoPolitico? returnEntity = await _partidoPoliticoRepository.UpdateAsync(entity.Id, entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<PartidoPoliticoDto>(returnEntity);
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
            try
            {
                var entity = await _partidoPoliticoRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<PartidoPoliticoDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<PartidoPoliticoDto>> GetAll()
        {
            try
            {
                var listEntities = await _partidoPoliticoRepository.GetAllAsync();
                return _mapper.Map<List<PartidoPoliticoDto>>(listEntities);
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<PartidoPoliticoDto?> GetBySiglasAsync(string siglas)
        {
            try
            {
                var entity = await _partidoPoliticoRepository.GetBySiglasAsync(siglas);
                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<PartidoPoliticoDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> HasActiveCandidatesAsync(int partidoPoliticoId)
        {
            try
            {
                return await _partidoPoliticoRepository.HasActiveCandidatesAsync(partidoPoliticoId);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
