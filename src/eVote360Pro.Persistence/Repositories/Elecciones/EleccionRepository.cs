using eVote360Pro.Core.Domain.Entities.Elecciones;
using eVote360Pro.Core.Domain.Enums;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Elecciones;
using eVote360Pro.Persistence.Contexts;
using eVote360Pro.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Persistence.Repositories.Elecciones
{
    public class EleccionRepository : GenericRepository<Eleccion>, IEleccionRepository
    {
        public EleccionRepository(eVote360ProContext context) : base(context)
        {
        }

        public async Task<Eleccion?> GetActiveElectionAsync()
        {
            return await _context.Elecciones
                .FirstOrDefaultAsync(e => e.Estado == EstadoEleccion.Activa);
        }

        public async Task<bool> HasActiveElectionAsync()
        {
            return await _context.Elecciones
                .AnyAsync(e => e.Estado == EstadoEleccion.Activa);
        }
    }
}
