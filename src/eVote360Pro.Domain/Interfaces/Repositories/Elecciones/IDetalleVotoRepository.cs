using eVote360Pro.Core.Domain.Entities.Elecciones;

namespace eVote360Pro.Core.Domain.Interfaces.Repositories.Elecciones
{
    public interface IDetalleVotoRepository : IGenericRepository<DetalleVoto>
    {
        Task<List<DetalleVoto>> GetByVotoAsync(int votoId);
    }
}
