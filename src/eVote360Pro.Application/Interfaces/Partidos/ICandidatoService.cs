using eVote360Pro.Core.Application.Dtos.Partidos;

namespace eVote360Pro.Core.Application.Interfaces.Partidos
{
    public interface ICandidatoService
    {
        Task<CandidatoDto?> AddAsync(SaveCandidatoDto dto);
        Task<CandidatoDto?> UpdateAsync(SaveCandidatoDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<CandidatoDto>> GetAll();
        Task<CandidatoDto?> GetById(int id);
        Task<bool> HasParticipatedInElectionAsync(int candidatoId);
        Task<bool> IsAssignedToPositionAsync(int candidatoId);
    }
}
