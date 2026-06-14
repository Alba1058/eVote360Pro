using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Elecciones;
using eVote360Pro.Core.Application.Interfaces.Elecciones;
using eVote360Pro.Core.Domain.Entities.Elecciones;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Elecciones;

namespace eVote360Pro.Core.Application.Services.Elecciones
{
    public class EleccionService : IEleccionService
    {
        private readonly IEleccionRepository _eleccionRepository;
        private readonly IMapper _mapper;

        public EleccionService(IEleccionRepository eleccionRepository, IMapper mapper)
        {
            _eleccionRepository = eleccionRepository;
            _mapper = mapper;
        }

        public async Task<EleccionDto?> AddAsync(SaveEleccionDto dto)
        {
            try
            {
                Eleccion entity = _mapper.Map<Eleccion>(dto);
                Eleccion? returnEntity = await _eleccionRepository.AddAsync(entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<EleccionDto>(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<EleccionDto?> UpdateAsync(SaveEleccionDto dto)
        {
            try
            {
                Eleccion entity = _mapper.Map<Eleccion>(dto);
                Eleccion? returnEntity = await _eleccionRepository.UpdateAsync(entity.Id, entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<EleccionDto>(returnEntity);
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
                await _eleccionRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<EleccionDto?> GetById(int id)
        {
            try
            {
                var entity = await _eleccionRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<EleccionDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<EleccionDto>> GetAll()
        {
            try
            {
                var listEntities = await _eleccionRepository.GetAllAsync();
                return _mapper.Map<List<EleccionDto>>(listEntities);
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<EleccionDto?> GetActiveElectionAsync()
        {
            try
            {
                var entity = await _eleccionRepository.GetActiveElectionAsync();
                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<EleccionDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> HasActiveElectionAsync()
        {
            try
            {
                return await _eleccionRepository.HasActiveElectionAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
