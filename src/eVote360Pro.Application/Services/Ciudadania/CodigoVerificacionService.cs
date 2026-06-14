using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Ciudadania;
using eVote360Pro.Core.Application.Interfaces.Ciudadania;
using eVote360Pro.Core.Domain.Entities.Ciudadania;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Ciudadania;

namespace eVote360Pro.Core.Application.Services.Ciudadania
{
    public class CodigoVerificacionService : ICodigoVerificacionService
    {
        private readonly ICodigoVerificacionRepository _codigoVerificacionRepository;
        private readonly IMapper _mapper;

        public CodigoVerificacionService(ICodigoVerificacionRepository codigoVerificacionRepository, IMapper mapper)
        {
            _codigoVerificacionRepository = codigoVerificacionRepository;
            _mapper = mapper;
        }

        public async Task<CodigoVerificacionDto?> AddAsync(CodigoVerificacionDto dto)
        {
            try
            {
                CodigoVerificacion entity = _mapper.Map<CodigoVerificacion>(dto);
                CodigoVerificacion? returnEntity = await _codigoVerificacionRepository.AddAsync(entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<CodigoVerificacionDto>(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<CodigoVerificacionDto?> UpdateAsync(CodigoVerificacionDto dto)
        {
            try
            {
                CodigoVerificacion entity = _mapper.Map<CodigoVerificacion>(dto);
                CodigoVerificacion? returnEntity = await _codigoVerificacionRepository.UpdateAsync(entity.Id, entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<CodigoVerificacionDto>(returnEntity);
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
                await _codigoVerificacionRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<CodigoVerificacionDto?> GetById(int id)
        {
            try
            {
                var entity = await _codigoVerificacionRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<CodigoVerificacionDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<CodigoVerificacionDto>> GetAll()
        {
            try
            {
                var listEntities = await _codigoVerificacionRepository.GetAllAsync();
                return _mapper.Map<List<CodigoVerificacionDto>>(listEntities);
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<CodigoVerificacionDto?> GetValidCodeAsync(int ciudadanoId, int eleccionId, string codigo)
        {
            try
            {
                var entity = await _codigoVerificacionRepository.GetValidCodeAsync(ciudadanoId, eleccionId, codigo);
                if (entity == null)
                {
                    return null;
                }

                return _mapper.Map<CodigoVerificacionDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<CodigoVerificacionDto?> GenerateCodeAsync(int ciudadanoId, int eleccionId)
        {
            try
            {
                Random random = new();
                string codigo = random.Next(100000, 999999).ToString();
                DateTime fechaGeneracion = DateTime.Now;
                DateTime fechaExpiracion = fechaGeneracion.AddMinutes(5);

                CodigoVerificacion entity = new()
                {
                    CiudadanoId = ciudadanoId,
                    EleccionId = eleccionId,
                    Codigo = codigo,
                    FechaGeneracion = fechaGeneracion,
                    FechaExpiracion = fechaExpiracion,
                    Usado = false,
                    IsActive = true
                };

                CodigoVerificacion? returnEntity = await _codigoVerificacionRepository.AddAsync(entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<CodigoVerificacionDto>(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
