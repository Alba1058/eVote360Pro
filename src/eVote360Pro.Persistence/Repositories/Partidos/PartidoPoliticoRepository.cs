using eVote360Pro.Core.Domain.Entities.Partidos;
using eVote360Pro.Core.Domain.Enums;
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

        public async Task<bool> HasParticipatedInElectionAsync(int partidoPoliticoId)
        {
            return await (
                from asignacion in _context.AsignacionCandidatos
                join ep in _context.EleccionPuestos on asignacion.PuestoElectivoId equals ep.PuestoElectivoId
                join eleccion in _context.Elecciones on ep.EleccionId equals eleccion.Id
                where asignacion.PartidoPoliticoId == partidoPoliticoId
                      && (eleccion.Estado == EstadoEleccion.Activa || eleccion.Estado == EstadoEleccion.Finalizada)
                select eleccion.Id
            ).AnyAsync();
        }
    }
}
