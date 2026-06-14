using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Alianzas;
using eVote360Pro.Core.Application.Interfaces.Alianzas;
using eVote360Pro.Core.Domain.Entities.Alianzas;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Alianzas;

namespace eVote360Pro.Core.Application.Services.Alianzas
{
    public class SolicitudAlianzaService : ISolicitudAlianzaService
    {
        private readonly ISolicitudAlianzaRepository _solicitudAlianzaRepository;
        private readonly IMapper _mapper;

        public SolicitudAlianzaService(ISolicitudAlianzaRepository solicitudAlianzaRepository, IMapper mapper)
        {
            _solicitudAlianzaRepository = solicitudAlianzaRepository;
            _mapper = mapper;
        }

        public async Task<SolicitudAlianzaDto?> AddAsync(SaveSolicitudAlianzaDto dto)
        {
            try
            {
                SolicitudAlianza entity = _mapper.Map<SolicitudAlianza>(dto);
                entity.FechaSolicitud = DateTime.Now;
                entity.Estado = Domain.Enums.EstadoSolicitudAlianza.EnEsperaDeRespuesta;

                SolicitudAlianza? returnEntity = await _solicitudAlianzaRepository.AddAsync(entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<SolicitudAlianzaDto>(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<SolicitudAlianzaDto?> UpdateAsync(SaveSolicitudAlianzaDto dto)
        {
            try
            {
                var existingEntity = await _solicitudAlianzaRepository.GetByIdAsync(dto.Id);
                if (existingEntity == null)
                {
                    return null;
                }

                SolicitudAlianza entity = _mapper.Map<SolicitudAlianza>(dto);
                entity.FechaSolicitud = existingEntity.FechaSolicitud;
                entity.Estado = existingEntity.Estado;

                SolicitudAlianza? returnEntity = await _solicitudAlianzaRepository.UpdateAsync(entity.Id, entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<SolicitudAlianzaDto>(returnEntity);
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
                await _solicitudAlianzaRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<SolicitudAlianzaDto?> GetById(int id)
        {
            try
            {
                var entity = await _solicitudAlianzaRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<SolicitudAlianzaDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<SolicitudAlianzaDto>> GetAll()
        {
            try
            {
                var listEntities = await _solicitudAlianzaRepository.GetAllAsync();
                return _mapper.Map<List<SolicitudAlianzaDto>>(listEntities);
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<List<SolicitudAlianzaDto>> GetPendingRequestsByReceiverAsync(int partidoReceptorId)
        {
            try
            {
                var listEntities = await _solicitudAlianzaRepository.GetPendingRequestsByReceiverAsync(partidoReceptorId);
                return _mapper.Map<List<SolicitudAlianzaDto>>(listEntities);
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<List<SolicitudAlianzaDto>> GetRequestsBySenderAsync(int partidoSolicitanteId)
        {
            try
            {
                var listEntities = await _solicitudAlianzaRepository.GetRequestsBySenderAsync(partidoSolicitanteId);
                return _mapper.Map<List<SolicitudAlianzaDto>>(listEntities);
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<bool> HasPendingRequestAsync(int partido1Id, int partido2Id)
        {
            try
            {
                return await _solicitudAlianzaRepository.HasPendingRequestAsync(partido1Id, partido2Id);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> HasActiveAllianceAsync(int partido1Id, int partido2Id)
        {
            try
            {
                return await _solicitudAlianzaRepository.HasActiveAllianceAsync(partido1Id, partido2Id);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
