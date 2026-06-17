using eVote360Pro.Core.Domain.Entities.Partidos;

namespace eVote360Pro.Core.Domain.Interfaces.Repositories.Partidos
{
    public interface IPartidoPoliticoRepository : IGenericRepository<PartidoPolitico>
    {
        Task<PartidoPolitico?> GetBySiglasAsync(string siglas);
        Task<bool> HasActiveCandidatesAsync(int partidoPoliticoId);
        Task<bool> HasParticipatedInElectionAsync(int partidoPoliticoId);
    }
}
