using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Alianzas;
using eVote360Pro.Core.Application.Interfaces.Alianzas;
using eVote360Pro.Core.Domain.Entities.Alianzas;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Alianzas;

namespace eVote360Pro.Core.Application.Services.Alianzas
{
    public class AlianzaPoliticaService : IAlianzaPoliticaService
    {
        private readonly IAlianzaPoliticaRepository _alianzaPoliticaRepository;
        private readonly IMapper _mapper;

        public AlianzaPoliticaService(IAlianzaPoliticaRepository alianzaPoliticaRepository, IMapper mapper)
        {
            _alianzaPoliticaRepository = alianzaPoliticaRepository;
            _mapper = mapper;
        }

        public async Task<AlianzaPoliticaDto?> AddAsync(SaveAlianzaPoliticaDto dto)
        {
            try
            {
                AlianzaPolitica entity = _mapper.Map<AlianzaPolitica>(dto);
                entity.FechaAceptacion = DateTime.Now;

                AlianzaPolitica? returnEntity = await _alianzaPoliticaRepository.AddAsync(entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<AlianzaPoliticaDto>(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<AlianzaPoliticaDto?> UpdateAsync(SaveAlianzaPoliticaDto dto)
        {
            try
            {
                var existingEntity = await _alianzaPoliticaRepository.GetByIdAsync(dto.Id);
                if (existingEntity == null)
                {
                    return null;
                }

                AlianzaPolitica entity = _mapper.Map<AlianzaPolitica>(dto);
                entity.FechaAceptacion = existingEntity.FechaAceptacion;

                AlianzaPolitica? returnEntity = await _alianzaPoliticaRepository.UpdateAsync(entity.Id, entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<AlianzaPoliticaDto>(returnEntity);
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
                await _alianzaPoliticaRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<AlianzaPoliticaDto?> GetById(int id)
        {
            try
            {
                var entity = await _alianzaPoliticaRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<AlianzaPoliticaDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<AlianzaPoliticaDto>> GetAll()
        {
            try
            {
                var listEntities = await _alianzaPoliticaRepository.GetAllAsync();
                return _mapper.Map<List<AlianzaPoliticaDto>>(listEntities);
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<AlianzaPoliticaDto?> GetByPartiesAsync(int partido1Id, int partido2Id)
        {
            try
            {
                var entity = await _alianzaPoliticaRepository.GetByPartiesAsync(partido1Id, partido2Id);
                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<AlianzaPoliticaDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> HasAlliedCandidateAssignmentsAsync(int partido1Id, int partido2Id)
        {
            try
            {
                return await _alianzaPoliticaRepository.HasAlliedCandidateAssignmentsAsync(partido1Id, partido2Id);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<AlianzaPoliticaDto>> GetVigentesByPartidoAsync(int partidoPoliticoId)
        {
            var all = await _alianzaPoliticaRepository.GetAllWithIncludeAsync(["Partido1", "Partido2"]);
            var filtradas = all.Where(a => a.IsActive && (a.Partido1Id == partidoPoliticoId || a.Partido2Id == partidoPoliticoId));
            return _mapper.Map<List<AlianzaPoliticaDto>>(filtradas);
        }

        public async Task<bool> EliminarAlianzaAsync(int alianzaId, int partidoPoliticoId)
        {
            var alianza = await _alianzaPoliticaRepository.GetByIdAsync(alianzaId);
            if (alianza == null || (!alianza.Partido1Id.Equals(partidoPoliticoId) && !alianza.Partido2Id.Equals(partidoPoliticoId)))
                return false;

            if (await _alianzaPoliticaRepository.HasAlliedCandidateAssignmentsAsync(alianza.Partido1Id, alianza.Partido2Id))
                return false;

            await _alianzaPoliticaRepository.DeleteAsync(alianzaId);
            return true;
        }
    }
}
