using eVote360Pro.Core.Application.Dtos.Alianzas;

namespace eVote360Pro.Core.Application.Interfaces.Alianzas
{
    public interface IAlianzaPoliticaService
    {
        Task<AlianzaPoliticaDto?> AddAsync(SaveAlianzaPoliticaDto dto);
        Task<AlianzaPoliticaDto?> UpdateAsync(SaveAlianzaPoliticaDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<AlianzaPoliticaDto>> GetAll();
        Task<AlianzaPoliticaDto?> GetById(int id);
        Task<AlianzaPoliticaDto?> GetByPartiesAsync(int partido1Id, int partido2Id);
        Task<bool> HasAlliedCandidateAssignmentsAsync(int partido1Id, int partido2Id);
        Task<List<AlianzaPoliticaDto>> GetVigentesByPartidoAsync(int partidoPoliticoId);
        Task<bool> EliminarAlianzaAsync(int alianzaId, int partidoPoliticoId);
    }
}
