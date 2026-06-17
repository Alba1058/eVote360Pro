using eVote360Pro.Core.Application.Dtos.Ciudadania;

namespace eVote360Pro.Core.Application.Interfaces.Ciudadania
{
    public interface ICiudadanoService
    {
        Task<CiudadanoDto?> AddAsync(SaveCiudadanoDto dto);
        Task<CiudadanoDto?> UpdateAsync(SaveCiudadanoDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<CiudadanoDto>> GetAll();
        Task<List<CiudadanoDto>> GetAllAsync() => GetAll();
        Task<CiudadanoDto?> GetById(int id);
        Task<CiudadanoDto?> GetByIdAsync(int id) => GetById(id);
        Task<CiudadanoDto?> GetByNumeroDocumentoAsync(string numeroDocumento);
        Task<bool> ExistsByNumeroDocumentoAsync(string numeroDocumento, int? excludeId = null);
        Task<bool> ExistsByCorreoAsync(string correo, int? excludeId = null);
        Task<bool> ActivateAsync(int id);
        Task<bool> DeactivateAsync(int id);
        Task<bool> HasVotedInElectionAsync(int ciudadanoId, int eleccionId);
        Task<bool> MarkAsVotedAsync(int ciudadanoId, int eleccionId);
        Task<bool> HasParticipatedInElectionAsync(int ciudadanoId);
    }
}
