using eVote360Pro.Core.Domain.Entities.Ciudadania;

namespace eVote360Pro.Core.Domain.Interfaces.Repositories.Ciudadania
{
    public interface ICiudadanoRepository : IGenericRepository<Ciudadano>
    {
        Task<Ciudadano?> GetByNumeroDocumentoAsync(string numeroDocumento);
        Task<bool> HasVotedInElectionAsync(int ciudadanoId, int eleccionId);
    }
}
