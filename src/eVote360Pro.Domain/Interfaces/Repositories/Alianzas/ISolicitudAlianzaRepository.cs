using eVote360Pro.Core.Domain.Entities.Alianzas;

namespace eVote360Pro.Core.Domain.Interfaces.Repositories.Alianzas
{
    public interface ISolicitudAlianzaRepository : IGenericRepository<SolicitudAlianza>
    {
        Task<List<SolicitudAlianza>> GetPendingRequestsByReceiverAsync(int partidoReceptorId);
        Task<List<SolicitudAlianza>> GetRequestsBySenderAsync(int partidoSolicitanteId);
        Task<bool> HasPendingRequestAsync(int partido1Id, int partido2Id);
        Task<bool> HasActiveAllianceAsync(int partido1Id, int partido2Id);
    }
}
