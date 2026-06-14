using eVote360Pro.Core.Application.Dtos.Puestos;

namespace eVote360Pro.Core.Application.Interfaces.Puestos
{
    public interface IPuestoElectivoService
    {
        Task<PuestoElectivoDto?> AddAsync(SavePuestoElectivoDto dto);
        Task<PuestoElectivoDto?> UpdateAsync(SavePuestoElectivoDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<PuestoElectivoDto>> GetAll();
        Task<PuestoElectivoDto?> GetById(int id);
        Task<PuestoElectivoDto?> GetByNombreAsync(string nombre);
        Task<bool> HasAssignedCandidatesAsync(int puestoElectivoId);
        Task<bool> HasParticipatedInElectionAsync(int puestoElectivoId);
    }
}
