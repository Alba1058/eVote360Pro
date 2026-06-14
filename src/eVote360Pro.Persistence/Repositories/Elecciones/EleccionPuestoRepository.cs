using eVote360Pro.Core.Domain.Entities.Elecciones;
using eVote360Pro.Core.Domain.Entities.Puestos;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Elecciones;
using eVote360Pro.Persistence.Contexts;
using eVote360Pro.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Persistence.Repositories.Elecciones
{
    public class EleccionPuestoRepository : GenericRepository<EleccionPuesto>, IEleccionPuestoRepository
    {
        public EleccionPuestoRepository(eVote360ProContext context) : base(context)
        {
        }

        public async Task<List<PuestoElectivo>> GetPuestosByEleccionAsync(int eleccionId)
        {
            return await _context.EleccionPuestos
                .Where(ep => ep.EleccionId == eleccionId)
                .Select(ep => ep.PuestoElectivo)
                .ToListAsync();
        }
    }
}
