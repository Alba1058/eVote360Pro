using eVote360Pro.Core.Application.Dtos.Elecciones;
using eVote360Pro.Core.Application.Dtos.Puestos;

namespace eVote360Pro.Core.Application.Interfaces.Elecciones
{
    public interface IEleccionService
    {
        Task<EleccionDto?> AddAsync(SaveEleccionDto dto);
        Task<EleccionDto?> UpdateAsync(SaveEleccionDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<EleccionDto>> GetAll();
        Task<List<EleccionDto>> GetAllAsync() => GetAll();
        Task<EleccionDto?> GetById(int id);
        Task<EleccionDto?> GetByIdAsync(int id) => GetById(id);
        Task<EleccionDto?> GetActiveElectionAsync();
        Task<bool> HasActiveElectionAsync();
        Task<bool> ActivateAsync(int id);
        Task<bool> FinalizeAsync(int id);
        Task<bool> HasPuestosAsync(int eleccionId);
        Task<bool> HasVotesAsync(int eleccionId);
        Task<List<int>> GetAniosDisponiblesAsync();
        Task<List<ResumenElectoralDto>> GetResumenByAnioAsync(int anio);
        Task<List<PuestoVotacionDto>> GetPuestosVotacionAsync(int eleccionId, Dictionary<int, int?> selecciones);
        Task<PuestoElectivoDto?> GetPuestoByIdAsync(int puestoId);
        Task<List<CandidatoVotacionDto>> GetCandidatosByPuestoAndEleccionAsync(int puestoId, int eleccionId);
        Task<List<ResultadoPuestoDto>> GetResultadosAsync(int eleccionId);
        Task<List<string>> ValidateConfigurationAsync();
        Task<ResumenElectoralDto?> GetResumenEleccionAsync(int eleccionId);
    }
}
