using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Elecciones;
using eVote360Pro.Core.Application.Dtos.Puestos;
using eVote360Pro.Core.Application.Interfaces.Elecciones;
using eVote360Pro.Core.Domain.Entities.Elecciones;
using eVote360Pro.Core.Domain.Entities.Puestos;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Elecciones;

namespace eVote360Pro.Core.Application.Services.Elecciones
{
    public class EleccionPuestoService : IEleccionPuestoService
    {
        private readonly IEleccionPuestoRepository _eleccionPuestoRepository;
        private readonly IMapper _mapper;

        public EleccionPuestoService(IEleccionPuestoRepository eleccionPuestoRepository, IMapper mapper)
        {
            _eleccionPuestoRepository = eleccionPuestoRepository;
            _mapper = mapper;
        }

        public async Task<EleccionPuestoDto?> AddAsync(SaveEleccionPuestoDto dto)
        {
            try
            {
                EleccionPuesto entity = _mapper.Map<EleccionPuesto>(dto);
                EleccionPuesto? returnEntity = await _eleccionPuestoRepository.AddAsync(entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<EleccionPuestoDto>(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<EleccionPuestoDto?> UpdateAsync(SaveEleccionPuestoDto dto)
        {
            try
            {
                EleccionPuesto entity = _mapper.Map<EleccionPuesto>(dto);
                EleccionPuesto? returnEntity = await _eleccionPuestoRepository.UpdateAsync(entity.Id, entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<EleccionPuestoDto>(returnEntity);
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
                await _eleccionPuestoRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<EleccionPuestoDto?> GetById(int id)
        {
            try
            {
                var entity = await _eleccionPuestoRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<EleccionPuestoDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<EleccionPuestoDto>> GetAll()
        {
            try
            {
                var listEntities = await _eleccionPuestoRepository.GetAllAsync();
                return _mapper.Map<List<EleccionPuestoDto>>(listEntities);
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<List<EleccionPuestoDto>> GetByEleccionAsync(int eleccionId)
        {
            try
            {
                var listEntities = await _eleccionPuestoRepository.GetPuestosByEleccionAsync(eleccionId);

                var listEntityDtos = listEntities.Select(s =>
                new EleccionPuestoDto()
                {
                    Id = 0,
                    EleccionId = eleccionId,
                    PuestoElectivoId = s.Id,
                    IsActive = s.IsActive
                }).ToList();

                return listEntityDtos;
            }
            catch (Exception)
            {
                return [];
            }
        }
    }
}
