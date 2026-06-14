using eVote360Pro.Core.Domain.Entities.Ciudadania;

namespace eVote360Pro.Core.Domain.Interfaces.Repositories.Ciudadania
{
    public interface ICodigoVerificacionRepository : IGenericRepository<CodigoVerificacion>
    {
        Task<CodigoVerificacion?> GetValidCodeAsync(int ciudadanoId, int eleccionId, string codigo);
    }
}
