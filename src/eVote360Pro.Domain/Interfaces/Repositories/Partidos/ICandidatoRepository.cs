using eVote360Pro.Core.Domain.Entities.Partidos;

namespace eVote360Pro.Core.Domain.Interfaces.Repositories.Partidos
{
    public interface ICandidatoRepository : IGenericRepository<Candidato>
    {
        Task<bool> HasParticipatedInElectionAsync(int candidatoId);
        Task<bool> IsAssignedToPositionAsync(int candidatoId);
    }
}
