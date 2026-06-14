using eVote360Pro.Core.Domain.Entities.Elecciones;

namespace eVote360Pro.Core.Domain.Interfaces.Repositories.Elecciones
{
    public interface IVotoRepository : IGenericRepository<Voto>
    {
        Task<Voto?> GetByCiudadanoAndEleccionAsync(int ciudadanoId, int eleccionId);
    }
}
