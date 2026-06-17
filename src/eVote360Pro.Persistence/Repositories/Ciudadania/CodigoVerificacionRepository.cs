using eVote360Pro.Core.Domain.Entities.Ciudadania;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Ciudadania;
using eVote360Pro.Persistence.Contexts;
using eVote360Pro.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace eVote360Pro.Persistence.Repositories.Ciudadania
{
    public class CodigoVerificacionRepository : GenericRepository<CodigoVerificacion>, ICodigoVerificacionRepository
    {
        public CodigoVerificacionRepository(eVote360ProContext context) : base(context)
        {
        }

        public async Task<CodigoVerificacion?> GetValidCodeAsync(int ciudadanoId, int eleccionId, string codigo)
        {
            return await _context.CodigoVerificaciones
                .FirstOrDefaultAsync(c => c.CiudadanoId == ciudadanoId &&
                                          c.EleccionId == eleccionId &&
                                          c.Codigo == codigo &&
                                          !c.Usado &&
                                          c.FechaExpiracion > DateTime.Now);
        }

        public async Task<CodigoVerificacion?> GetByCiudadanoEleccionAndCodigoAsync(int ciudadanoId, int eleccionId, string codigo)
        {
            return await _context.CodigoVerificaciones
                .OrderByDescending(c => c.FechaGeneracion)
                .FirstOrDefaultAsync(c => c.CiudadanoId == ciudadanoId &&
                                          c.EleccionId == eleccionId &&
                                          c.Codigo == codigo);
        }
    }
}
