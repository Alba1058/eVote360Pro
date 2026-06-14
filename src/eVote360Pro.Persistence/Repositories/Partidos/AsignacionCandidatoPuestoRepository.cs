using eVote360Pro.Core.Domain.Entities.Partidos;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Partidos;
using eVote360Pro.Persistence.Contexts;
using eVote360Pro.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Persistence.Repositories.Partidos
{
    public class AsignacionCandidatoPuestoRepository : GenericRepository<AsignacionCandidatoPuesto>, IAsignacionCandidatoPuestoRepository
    {
        public AsignacionCandidatoPuestoRepository(eVote360ProContext context) : base(context)
        {
        }

        public async Task<List<AsignacionCandidatoPuesto>> GetByPartidoPoliticoAsync(int partidoPoliticoId)
        {
            return await _context.AsignacionCandidatos
                .Where(a => a.PartidoPoliticoId == partidoPoliticoId)
                .ToListAsync();
        }

        public async Task<bool> CandidateAssignedToPartyAsync(int candidatoId, int partidoPoliticoId)
        {
            return await _context.AsignacionCandidatos
                .AnyAsync(a => a.CandidatoId == candidatoId && a.PartidoPoliticoId == partidoPoliticoId);
        }

        public async Task<bool> PositionOccupiedInPartyAsync(int puestoElectivoId, int partidoPoliticoId)
        {
            return await _context.AsignacionCandidatos
                .AnyAsync(a => a.PuestoElectivoId == puestoElectivoId && a.PartidoPoliticoId == partidoPoliticoId);
        }
    }
}
