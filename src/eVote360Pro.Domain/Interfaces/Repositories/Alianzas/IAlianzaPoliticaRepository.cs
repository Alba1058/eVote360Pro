using eVote360Pro.Core.Domain.Entities.Alianzas;

namespace eVote360Pro.Core.Domain.Interfaces.Repositories.Alianzas
{
    public interface IAlianzaPoliticaRepository : IGenericRepository<AlianzaPolitica>
    {
        Task<AlianzaPolitica?> GetByPartiesAsync(int partido1Id, int partido2Id);
        Task<bool> HasAlliedCandidateAssignmentsAsync(int partido1Id, int partido2Id);
    }
}
