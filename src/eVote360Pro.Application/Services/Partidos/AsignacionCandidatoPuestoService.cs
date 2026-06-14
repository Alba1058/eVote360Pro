using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Partidos;
using eVote360Pro.Core.Application.Interfaces.Partidos;
using eVote360Pro.Core.Domain.Entities.Partidos;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Partidos;

namespace eVote360Pro.Core.Application.Services.Partidos
{
    public class AsignacionCandidatoPuestoService : IAsignacionCandidatoPuestoService
    {
        private readonly IAsignacionCandidatoPuestoRepository _asignacionCandidatoPuestoRepository;
        private readonly IMapper _mapper;

        public AsignacionCandidatoPuestoService(IAsignacionCandidatoPuestoRepository asignacionCandidatoPuestoRepository, IMapper mapper)
        {
            _asignacionCandidatoPuestoRepository = asignacionCandidatoPuestoRepository;
            _mapper = mapper;
        }

        public async Task<AsignacionCandidatoPuestoDto?> AddAsync(SaveAsignacionCandidatoPuestoDto dto)
        {
            try
            {
                AsignacionCandidatoPuesto entity = _mapper.Map<AsignacionCandidatoPuesto>(dto);
                AsignacionCandidatoPuesto? returnEntity = await _asignacionCandidatoPuestoRepository.AddAsync(entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<AsignacionCandidatoPuestoDto>(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<AsignacionCandidatoPuestoDto?> UpdateAsync(SaveAsignacionCandidatoPuestoDto dto)
        {
            try
            {
                AsignacionCandidatoPuesto entity = _mapper.Map<AsignacionCandidatoPuesto>(dto);
                AsignacionCandidatoPuesto? returnEntity = await _asignacionCandidatoPuestoRepository.UpdateAsync(entity.Id, entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<AsignacionCandidatoPuestoDto>(returnEntity);
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
                await _asignacionCandidatoPuestoRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<AsignacionCandidatoPuestoDto?> GetById(int id)
        {
            try
            {
                var entity = await _asignacionCandidatoPuestoRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<AsignacionCandidatoPuestoDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<AsignacionCandidatoPuestoDto>> GetAll()
        {
            try
            {
                var listEntities = await _asignacionCandidatoPuestoRepository.GetAllAsync();
                return _mapper.Map<List<AsignacionCandidatoPuestoDto>>(listEntities);
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<List<AsignacionCandidatoPuestoDto>> GetByPartidoPoliticoAsync(int partidoPoliticoId)
        {
            try
            {
                var listEntities = await _asignacionCandidatoPuestoRepository.GetByPartidoPoliticoAsync(partidoPoliticoId);
                return _mapper.Map<List<AsignacionCandidatoPuestoDto>>(listEntities);
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<bool> CandidateAssignedToPartyAsync(int candidatoId, int partidoPoliticoId)
        {
            try
            {
                return await _asignacionCandidatoPuestoRepository.CandidateAssignedToPartyAsync(candidatoId, partidoPoliticoId);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> PositionOccupiedInPartyAsync(int puestoElectivoId, int partidoPoliticoId)
        {
            try
            {
                return await _asignacionCandidatoPuestoRepository.PositionOccupiedInPartyAsync(puestoElectivoId, partidoPoliticoId);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
