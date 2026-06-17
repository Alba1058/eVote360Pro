using eVote360Pro.Core.Application.Dtos.Ciudadania;

namespace eVote360Pro.Core.Application.Interfaces.Ciudadania
{
    public interface ICodigoVerificacionService
    {
        Task<CodigoVerificacionDto?> AddAsync(CodigoVerificacionDto dto);
        Task<CodigoVerificacionDto?> UpdateAsync(CodigoVerificacionDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<CodigoVerificacionDto>> GetAll();
        Task<CodigoVerificacionDto?> GetById(int id);
        Task<CodigoVerificacionDto?> GetValidCodeAsync(int ciudadanoId, int eleccionId, string codigo);
        Task<CodigoVerificacionDto?> GenerateCodeAsync(int ciudadanoId, int eleccionId);
        Task<bool> ValidateCodeAsync(int ciudadanoId, int eleccionId, string codigo);
        Task<string?> ValidateCodeWithMessageAsync(int ciudadanoId, int eleccionId, string codigo);
    }
}
