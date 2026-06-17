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

        public override async Task<List<AsignacionCandidatoPuesto>> GetAllAsync()
        {
            return await _context.AsignacionCandidatos
                .Include(a => a.Candidato)
                .Include(a => a.PuestoElectivo)
                .Include(a => a.PartidoPolitico)
                .ToListAsync();
        }

        public async Task<AsignacionCandidatoPuesto?> GetByIdWithIncludesAsync(int id)
        {
            return await _context.AsignacionCandidatos
                .Include(a => a.Candidato)
                .Include(a => a.PuestoElectivo)
                .Include(a => a.PartidoPolitico)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<AsignacionCandidatoPuesto>> GetByPartidoPoliticoAsync(int partidoPoliticoId)
        {
            return await _context.AsignacionCandidatos
                .Include(a => a.Candidato)
                .Include(a => a.PuestoElectivo)
                .Include(a => a.PartidoPolitico)
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

        public async Task ActivateAsync(int id)
        {
            var entity = await _context.AsignacionCandidatos.FindAsync(id);
            if (entity != null)
            {
                entity.IsActive = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeactivateAsync(int id)
        {
            var entity = await _context.AsignacionCandidatos.FindAsync(id);
            if (entity != null)
            {
                entity.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
