using eVote360Pro.Core.Application.Dtos.Elecciones;

namespace eVote360Pro.Core.Application.Interfaces.Elecciones
{
    public interface IEleccionService
    {
        Task<EleccionDto?> AddAsync(SaveEleccionDto dto);
        Task<EleccionDto?> UpdateAsync(SaveEleccionDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<EleccionDto>> GetAll();
        Task<EleccionDto?> GetById(int id);
        Task<EleccionDto?> GetActiveElectionAsync();
        Task<bool> HasActiveElectionAsync();
    }
}
