using eVote360Pro.Core.Application.Dtos.Ciudadania;

namespace eVote360Pro.Core.Application.Interfaces.Ciudadania
{
    public interface ICiudadanoService
    {
        Task<CiudadanoDto?> AddAsync(SaveCiudadanoDto dto);
        Task<CiudadanoDto?> UpdateAsync(SaveCiudadanoDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<CiudadanoDto>> GetAll();
        Task<CiudadanoDto?> GetById(int id);
        Task<CiudadanoDto?> GetByNumeroDocumentoAsync(string numeroDocumento);
        Task<bool> HasVotedInElectionAsync(int ciudadanoId, int eleccionId);
    }
}
