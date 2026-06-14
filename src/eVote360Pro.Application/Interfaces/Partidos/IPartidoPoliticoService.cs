using eVote360Pro.Core.Application.Dtos.Partidos;

namespace eVote360Pro.Core.Application.Interfaces.Partidos
{
    public interface IPartidoPoliticoService
    {
        Task<PartidoPoliticoDto?> AddAsync(SavePartidoPoliticoDto dto);
        Task<PartidoPoliticoDto?> UpdateAsync(SavePartidoPoliticoDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<PartidoPoliticoDto>> GetAll();
        Task<PartidoPoliticoDto?> GetById(int id);
        Task<PartidoPoliticoDto?> GetBySiglasAsync(string siglas);
        Task<bool> HasActiveCandidatesAsync(int partidoPoliticoId);
    }
}
