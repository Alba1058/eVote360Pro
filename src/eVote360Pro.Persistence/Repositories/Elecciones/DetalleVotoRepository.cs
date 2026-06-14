using eVote360Pro.Core.Domain.Entities.Elecciones;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Elecciones;
using eVote360Pro.Persistence.Contexts;
using eVote360Pro.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Persistence.Repositories.Elecciones
{
    public class DetalleVotoRepository : GenericRepository<DetalleVoto>, IDetalleVotoRepository
    {
        public DetalleVotoRepository(eVote360ProContext context) : base(context)
        {
        }

        public async Task<List<DetalleVoto>> GetByVotoAsync(int votoId)
        {
            return await _context.DetalleVotos
                .Where(d => d.VotoId == votoId)
                .ToListAsync();
        }
    }
}
