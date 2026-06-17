using eVote360Pro.Core.Application.Dtos.Partidos;

namespace eVote360Pro.Core.Application.Interfaces.Partidos
{
    public interface IPartidoPoliticoService
    {
        Task<PartidoPoliticoDto?> AddAsync(SavePartidoPoliticoDto dto);
        Task<PartidoPoliticoDto?> UpdateAsync(SavePartidoPoliticoDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<PartidoPoliticoDto>> GetAll();
        Task<List<PartidoPoliticoDto>> GetAllAsync() => GetAll();
        Task<PartidoPoliticoDto?> GetById(int id);
        Task<PartidoPoliticoDto?> GetByIdAsync(int id) => GetById(id);
        Task<List<PartidoPoliticoDto>> GetAllActiveAsync();
        Task<PartidoPoliticoDto?> GetBySiglasAsync(string siglas);
        Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null);
        Task<bool> ExistsBySiglasAsync(string siglas, int? excludeId = null);
        Task<bool> HasActiveCandidatesAsync(int partidoPoliticoId);
        Task<bool> HasActiveLeadersAsync(int partidoPoliticoId);
        Task<bool> HasParticipatedInElectionAsync(int partidoPoliticoId);
        Task<bool> ActivateAsync(int id);
        Task<bool> DeactivateAsync(int id);
    }
}
