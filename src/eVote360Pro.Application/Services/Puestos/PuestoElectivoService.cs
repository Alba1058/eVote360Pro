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
                entity.IsActive = dto.IsActive;
                PuestoElectivo? returnEntity = await _puestoElectivoRepository.AddAsync(entity);
                return returnEntity == null ? null : _mapper.Map<PuestoElectivoDto>(returnEntity);
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
                var existing = await _puestoElectivoRepository.GetByIdAsync(dto.Id);
                if (existing == null) return null;

                if (await _puestoElectivoRepository.HasParticipatedInElectionAsync(dto.Id))
                {
                    existing.Descripcion = dto.Descripcion;
                    existing.IsActive = dto.IsActive;
                }
                else
                {
                    existing.Nombre = dto.Nombre.Trim();
                    existing.Descripcion = dto.Descripcion;
                    existing.IsActive = dto.IsActive;
                }

                existing.UpdatedAt = DateTime.UtcNow;
                var returnEntity = await _puestoElectivoRepository.UpdateAsync(existing.Id, existing);
                return returnEntity == null ? null : _mapper.Map<PuestoElectivoDto>(returnEntity);
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
                return entity == null ? null : _mapper.Map<PuestoElectivoDto>(entity);
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

        public async Task<List<PuestoElectivoDto>> GetAllActiveAsync()
        {
            var all = await GetAll();
            return all.Where(p => p.IsActive).ToList();
        }

        public async Task<PuestoElectivoDto?> GetByNombreAsync(string nombre)
        {
            try
            {
                var entity = await _puestoElectivoRepository.GetByNombreAsync(nombre.Trim());
                return entity == null ? null : _mapper.Map<PuestoElectivoDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null)
        {
            var entity = await _puestoElectivoRepository.GetByNombreAsync(nombre.Trim());
            if (entity == null) return false;
            return excludeId == null || entity.Id != excludeId;
        }

        public async Task<bool> ActivateAsync(int id)
        {
            var entity = await _puestoElectivoRepository.GetByIdAsync(id);
            if (entity == null || entity.IsActive) return false;
            entity.IsActive = true;
            entity.UpdatedAt = DateTime.UtcNow;
            await _puestoElectivoRepository.UpdateAsync(id, entity);
            return true;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var entity = await _puestoElectivoRepository.GetByIdAsync(id);
            if (entity == null || !entity.IsActive) return false;
            entity.IsActive = false;
            entity.UpdatedAt = DateTime.UtcNow;
            await _puestoElectivoRepository.UpdateAsync(id, entity);
            return true;
        }

        public async Task<bool> HasAssignedCandidatesAsync(int puestoElectivoId)
        {
            return await _puestoElectivoRepository.HasAssignedCandidatesAsync(puestoElectivoId);
        }

        public async Task<bool> HasParticipatedInElectionAsync(int puestoElectivoId)
        {
            return await _puestoElectivoRepository.HasParticipatedInElectionAsync(puestoElectivoId);
        }
    }
}
