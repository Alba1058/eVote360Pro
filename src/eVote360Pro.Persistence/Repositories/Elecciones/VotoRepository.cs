using eVote360Pro.Core.Domain.Entities.Elecciones;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Elecciones;
using eVote360Pro.Persistence.Contexts;
using eVote360Pro.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Persistence.Repositories.Elecciones
{
    public class VotoRepository : GenericRepository<Voto>, IVotoRepository
    {
        public VotoRepository(eVote360ProContext context) : base(context)
        {
        }

        public async Task<Voto?> GetByCiudadanoAndEleccionAsync(int ciudadanoId, int eleccionId)
        {
            return await _context.Votos
                .FirstOrDefaultAsync(v => v.CiudadanoId == ciudadanoId && v.EleccionId == eleccionId);
        }
    }
}
