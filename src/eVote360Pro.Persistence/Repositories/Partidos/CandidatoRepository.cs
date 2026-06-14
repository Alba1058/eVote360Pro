using eVote360Pro.Core.Domain.Entities.Partidos;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Partidos;
using eVote360Pro.Persistence.Contexts;
using eVote360Pro.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Persistence.Repositories.Partidos
{
    public class CandidatoRepository : GenericRepository<Candidato>, ICandidatoRepository
    {
        public CandidatoRepository(eVote360ProContext context) : base(context)
        {
        }

        public async Task<bool> HasParticipatedInElectionAsync(int candidatoId)
        {
            return await _context.DetalleVotos
                .AnyAsync(d => d.CandidatoId == candidatoId);
        }

        public async Task<bool> IsAssignedToPositionAsync(int candidatoId)
        {
            return await _context.AsignacionCandidatos
                .AnyAsync(a => a.CandidatoId == candidatoId);
        }
    }
}
