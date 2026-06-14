using eVote360Pro.Core.Domain.Entities.Puestos;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Puestos;
using eVote360Pro.Persistence.Contexts;
using eVote360Pro.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Persistence.Repositories.Puestos
{
    public class PuestoElectivoRepository : GenericRepository<PuestoElectivo>, IPuestoElectivoRepository
    {
        public PuestoElectivoRepository(eVote360ProContext context) : base(context)
        {
        }

        public async Task<PuestoElectivo?> GetByNombreAsync(string nombre)
        {
            return await _context.PuestoElectivos
                .FirstOrDefaultAsync(p => p.Nombre == nombre);
        }

        public async Task<bool> HasAssignedCandidatesAsync(int puestoElectivoId)
        {
            return await _context.AsignacionCandidatos
                .AnyAsync(a => a.PuestoElectivoId == puestoElectivoId);
        }

        public async Task<bool> HasParticipatedInElectionAsync(int puestoElectivoId)
        {
            return await _context.EleccionPuestos
                .AnyAsync(ep => ep.PuestoElectivoId == puestoElectivoId);
        }
    }
}
