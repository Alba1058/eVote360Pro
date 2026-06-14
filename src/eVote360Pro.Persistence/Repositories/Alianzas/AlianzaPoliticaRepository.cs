using eVote360Pro.Core.Domain.Entities.Alianzas;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Alianzas;
using eVote360Pro.Persistence.Contexts;
using eVote360Pro.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Persistence.Repositories.Alianzas
{
    public class AlianzaPoliticaRepository : GenericRepository<AlianzaPolitica>, IAlianzaPoliticaRepository
    {
        public AlianzaPoliticaRepository(eVote360ProContext context) : base(context)
        {
        }

        public async Task<AlianzaPolitica?> GetByPartiesAsync(int partido1Id, int partido2Id)
        {
            return await _context.AlianzaPoliticas
                .FirstOrDefaultAsync(a => (a.Partido1Id == partido1Id && a.Partido2Id == partido2Id) ||
                                          (a.Partido1Id == partido2Id && a.Partido2Id == partido1Id));
        }

        public async Task<bool> HasAlliedCandidateAssignmentsAsync(int partido1Id, int partido2Id)
        {
            return await _context.AsignacionCandidatos
                .AnyAsync(a => a.PartidoPoliticoId == partido1Id || a.PartidoPoliticoId == partido2Id);
        }
    }
}
