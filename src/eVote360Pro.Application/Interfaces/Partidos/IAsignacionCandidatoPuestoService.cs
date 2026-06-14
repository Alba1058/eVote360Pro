using eVote360Pro.Core.Application.Dtos.Partidos;

namespace eVote360Pro.Core.Application.Interfaces.Partidos
{
    public interface IAsignacionCandidatoPuestoService
    {
        Task<AsignacionCandidatoPuestoDto?> AddAsync(SaveAsignacionCandidatoPuestoDto dto);
        Task<AsignacionCandidatoPuestoDto?> UpdateAsync(SaveAsignacionCandidatoPuestoDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<AsignacionCandidatoPuestoDto>> GetAll();
        Task<AsignacionCandidatoPuestoDto?> GetById(int id);
        Task<List<AsignacionCandidatoPuestoDto>> GetByPartidoPoliticoAsync(int partidoPoliticoId);
        Task<bool> CandidateAssignedToPartyAsync(int candidatoId, int partidoPoliticoId);
        Task<bool> PositionOccupiedInPartyAsync(int puestoElectivoId, int partidoPoliticoId);
    }
}
