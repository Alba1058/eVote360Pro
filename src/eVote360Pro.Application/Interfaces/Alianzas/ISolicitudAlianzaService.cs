using eVote360Pro.Core.Application.Dtos.Alianzas;

namespace eVote360Pro.Core.Application.Interfaces.Alianzas
{
    public interface ISolicitudAlianzaService
    {
        Task<SolicitudAlianzaDto?> AddAsync(SaveSolicitudAlianzaDto dto);
        Task<SolicitudAlianzaDto?> UpdateAsync(SaveSolicitudAlianzaDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<SolicitudAlianzaDto>> GetAll();
        Task<SolicitudAlianzaDto?> GetById(int id);
        Task<List<SolicitudAlianzaDto>> GetPendingRequestsByReceiverAsync(int partidoReceptorId);
        Task<List<SolicitudAlianzaDto>> GetRequestsBySenderAsync(int partidoSolicitanteId);
        Task<bool> HasPendingRequestAsync(int partido1Id, int partido2Id);
        Task<bool> HasActiveAllianceAsync(int partido1Id, int partido2Id);
        Task<bool> AceptarAsync(int solicitudId, int partidoReceptorId);
        Task<bool> RechazarAsync(int solicitudId, int partidoReceptorId);
    }
}
