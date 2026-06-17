using eVote360Pro.Core.Domain.Entities.Alianzas;
using eVote360Pro.Core.Domain.Enums;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Alianzas;
using eVote360Pro.Persistence.Contexts;
using eVote360Pro.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Persistence.Repositories.Alianzas
{
    public class SolicitudAlianzaRepository : GenericRepository<SolicitudAlianza>, ISolicitudAlianzaRepository
    {
        public SolicitudAlianzaRepository(eVote360ProContext context) : base(context)
        {
        }

        public async Task<List<SolicitudAlianza>> GetPendingRequestsByReceiverAsync(int partidoReceptorId)
        {
            return await _context.SolicitudAlianzas
                .Include(s => s.PartidoSolicitante)
                .Include(s => s.PartidoReceptor)
                .Where(s => s.PartidoReceptorId == partidoReceptorId && s.Estado == EstadoSolicitudAlianza.EnEsperaDeRespuesta)
                .ToListAsync();
        }

        public async Task<List<SolicitudAlianza>> GetRequestsBySenderAsync(int partidoSolicitanteId)
        {
            return await _context.SolicitudAlianzas
                .Include(s => s.PartidoSolicitante)
                .Include(s => s.PartidoReceptor)
                .Where(s => s.PartidoSolicitanteId == partidoSolicitanteId)
                .ToListAsync();
        }

        public async Task<bool> HasPendingRequestAsync(int partido1Id, int partido2Id)
        {
            return await _context.SolicitudAlianzas
                .AnyAsync(s => s.Estado == EstadoSolicitudAlianza.EnEsperaDeRespuesta &&
                               ((s.PartidoSolicitanteId == partido1Id && s.PartidoReceptorId == partido2Id) ||
                                (s.PartidoSolicitanteId == partido2Id && s.PartidoReceptorId == partido1Id)));
        }

        public async Task<bool> HasActiveAllianceAsync(int partido1Id, int partido2Id)
        {
            return await _context.SolicitudAlianzas
                .AnyAsync(s => s.Estado == EstadoSolicitudAlianza.Aceptada &&
                               ((s.PartidoSolicitanteId == partido1Id && s.PartidoReceptorId == partido2Id) ||
                                (s.PartidoSolicitanteId == partido2Id && s.PartidoReceptorId == partido1Id)));
        }
    }
}
