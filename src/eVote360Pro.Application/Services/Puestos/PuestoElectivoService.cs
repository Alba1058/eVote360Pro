using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Puestos;
using eVote360Pro.Core.Application.Interfaces.Puestos;
using eVote360Pro.Core.Domain.Entities.Puestos;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Puestos;

namespace eVote360Pro.Core.Application.Services.Puestos
{
    public class PuestoElectivoService : IPuestoElectivoService
    {
        private readonly IPuestoElectivoRepository _puestoElectivoRepository;
        private readonly IMapper _mapper;

        public PuestoElectivoService(IPuestoElectivoRepository puestoElectivoRepository, IMapper mapper)
        {
            _puestoElectivoRepository = puestoElectivoRepository;
            _mapper = mapper;
        }

        public async Task<PuestoElectivoDto?> AddAsync(SavePuestoElectivoDto dto)
        {
            try
            {
                PuestoElectivo entity = _mapper.Map<PuestoElectivo>(dto);
                PuestoElectivo? returnEntity = await _puestoElectivoRepository.AddAsync(entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<PuestoElectivoDto>(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<PuestoElectivoDto?> UpdateAsync(SavePuestoElectivoDto dto)
        {
            try
            {
                PuestoElectivo entity = _mapper.Map<PuestoElectivo>(dto);
                PuestoElectivo? returnEntity = await _puestoElectivoRepository.UpdateAsync(entity.Id, entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<PuestoElectivoDto>(returnEntity);
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
                await _puestoElectivoRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<PuestoElectivoDto?> GetById(int id)
        {
            try
            {
                var entity = await _puestoElectivoRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<PuestoElectivoDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<PuestoElectivoDto>> GetAll()
        {
            try
            {
                var listEntities = await _puestoElectivoRepository.GetAllAsync();
                return _mapper.Map<List<PuestoElectivoDto>>(listEntities);
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<PuestoElectivoDto?> GetByNombreAsync(string nombre)
        {
            try
            {
                var entity = await _puestoElectivoRepository.GetByNombreAsync(nombre);
                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<PuestoElectivoDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> HasAssignedCandidatesAsync(int puestoElectivoId)
        {
            try
            {
                return await _puestoElectivoRepository.HasAssignedCandidatesAsync(puestoElectivoId);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> HasParticipatedInElectionAsync(int puestoElectivoId)
        {
            try
            {
                return await _puestoElectivoRepository.HasParticipatedInElectionAsync(puestoElectivoId);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
