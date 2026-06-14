using eVote360Pro.Core.Application.Dtos.Elecciones;

namespace eVote360Pro.Core.Application.Interfaces.Elecciones
{
    public interface IEleccionPuestoService
    {
        Task<EleccionPuestoDto?> AddAsync(SaveEleccionPuestoDto dto);
        Task<EleccionPuestoDto?> UpdateAsync(SaveEleccionPuestoDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<EleccionPuestoDto>> GetAll();
        Task<EleccionPuestoDto?> GetById(int id);
        Task<List<EleccionPuestoDto>> GetByEleccionAsync(int eleccionId);
    }
}
