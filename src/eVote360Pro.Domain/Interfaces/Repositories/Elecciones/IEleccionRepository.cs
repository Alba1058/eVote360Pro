using eVote360Pro.Core.Domain.Entities.Elecciones;

namespace eVote360Pro.Core.Domain.Interfaces.Repositories.Elecciones
{
    public interface IEleccionRepository : IGenericRepository<Eleccion>
    {
        Task<Eleccion?> GetActiveElectionAsync();
        Task<bool> HasActiveElectionAsync();
    }
}
