using eVote360Pro.Core.Domain.Entities.Ciudadania;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Ciudadania;
using eVote360Pro.Persistence.Contexts;
using eVote360Pro.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Persistence.Repositories.Ciudadania
{
    public class CiudadanoRepository : GenericRepository<Ciudadano>, ICiudadanoRepository
    {
        public CiudadanoRepository(eVote360ProContext context) : base(context)
        {
        }

        public async Task<Ciudadano?> GetByNumeroDocumentoAsync(string numeroDocumento)
        {
            return await _context.Ciudadanos
                .FirstOrDefaultAsync(c => c.NumeroDocumento == numeroDocumento);
        }

        public async Task<bool> HasVotedInElectionAsync(int ciudadanoId, int eleccionId)
        {
            return await _context.Votos
                .AnyAsync(v => v.CiudadanoId == ciudadanoId && v.EleccionId == eleccionId);
        }
    }
}
