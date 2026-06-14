using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Ciudadania;
using eVote360Pro.Core.Application.Interfaces.Ciudadania;
using eVote360Pro.Core.Domain.Entities.Ciudadania;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Ciudadania;

namespace eVote360Pro.Core.Application.Services.Ciudadania
{
    public class CiudadanoService : ICiudadanoService
    {
        private readonly ICiudadanoRepository _ciudadanoRepository;
        private readonly IMapper _mapper;

        public CiudadanoService(ICiudadanoRepository ciudadanoRepository, IMapper mapper)
        {
            _ciudadanoRepository = ciudadanoRepository;
            _mapper = mapper;
        }

        public async Task<CiudadanoDto?> AddAsync(SaveCiudadanoDto dto)
        {
            try
            {
                Ciudadano entity = _mapper.Map<Ciudadano>(dto);
                Ciudadano? returnEntity = await _ciudadanoRepository.AddAsync(entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<CiudadanoDto>(returnEntity);
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
                Ciudadano entity = _mapper.Map<Ciudadano>(dto);
                Ciudadano? returnEntity = await _ciudadanoRepository.UpdateAsync(entity.Id, entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<CiudadanoDto>(returnEntity);
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
                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<CiudadanoDto>(entity);
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
                var entity = await _ciudadanoRepository.GetByNumeroDocumentoAsync(numeroDocumento);
                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<CiudadanoDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> HasVotedInElectionAsync(int ciudadanoId, int eleccionId)
        {
            try
            {
                return await _ciudadanoRepository.HasVotedInElectionAsync(ciudadanoId, eleccionId);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
