using eVote360Pro.Core.Domain.Entities.Partidos;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Partidos;
using eVote360Pro.Persistence.Contexts;
using eVote360Pro.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Persistence.Repositories.Partidos
{
    public class PartidoPoliticoRepository : GenericRepository<PartidoPolitico>, IPartidoPoliticoRepository
    {
        public PartidoPoliticoRepository(eVote360ProContext context) : base(context)
        {
        }

        public async Task<PartidoPolitico?> GetBySiglasAsync(string siglas)
        {
            return await _context.PartidoPoliticos
                .FirstOrDefaultAsync(p => p.Siglas == siglas);
        }

        public async Task<bool> HasActiveCandidatesAsync(int partidoPoliticoId)
        {
            return await _context.Candidatos
                .AnyAsync(c => c.PartidoPoliticoId == partidoPoliticoId && c.IsActive);
        }
    }
}
