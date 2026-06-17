using eVote360Pro.Core.Application.Dtos.Puestos;

namespace eVote360Pro.Core.Application.Interfaces.Puestos
{
    public interface IPuestoElectivoService
    {
        Task<PuestoElectivoDto?> AddAsync(SavePuestoElectivoDto dto);
        Task<PuestoElectivoDto?> UpdateAsync(SavePuestoElectivoDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<PuestoElectivoDto>> GetAll();
        Task<List<PuestoElectivoDto>> GetAllAsync() => GetAll();
        Task<PuestoElectivoDto?> GetById(int id);
        Task<PuestoElectivoDto?> GetByIdAsync(int id) => GetById(id);
        Task<List<PuestoElectivoDto>> GetAllActiveAsync();
        Task<PuestoElectivoDto?> GetByNombreAsync(string nombre);
        Task<bool> ExistsByNameAsync(string nombre, int? excludeId = null);
        Task<bool> ActivateAsync(int id);
        Task<bool> DeactivateAsync(int id);
        Task<bool> HasAssignedCandidatesAsync(int puestoElectivoId);
        Task<bool> HasCandidatesAsync(int puestoElectivoId) => HasAssignedCandidatesAsync(puestoElectivoId);
        Task<bool> HasParticipatedInElectionAsync(int puestoElectivoId);
    }
}
