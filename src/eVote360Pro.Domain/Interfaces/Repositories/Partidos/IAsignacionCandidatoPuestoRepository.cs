using eVote360Pro.Core.Domain.Entities.Partidos;

namespace eVote360Pro.Core.Domain.Interfaces.Repositories.Partidos
{
    public interface IAsignacionCandidatoPuestoRepository : IGenericRepository<AsignacionCandidatoPuesto>
    {
        Task<List<AsignacionCandidatoPuesto>> GetByPartidoPoliticoAsync(int partidoPoliticoId);
        Task<bool> CandidateAssignedToPartyAsync(int candidatoId, int partidoPoliticoId);
        Task<bool> PositionOccupiedInPartyAsync(int puestoElectivoId, int partidoPoliticoId);
    }
}
