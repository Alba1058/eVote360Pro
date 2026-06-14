using eVote360Pro.Core.Domain.Entities.Puestos;

namespace eVote360Pro.Core.Domain.Interfaces.Repositories.Puestos
{
    public interface IPuestoElectivoRepository : IGenericRepository<PuestoElectivo>
    {
        Task<PuestoElectivo?> GetByNombreAsync(string nombre);
        Task<bool> HasAssignedCandidatesAsync(int puestoElectivoId);
        Task<bool> HasParticipatedInElectionAsync(int puestoElectivoId);
    }
}
