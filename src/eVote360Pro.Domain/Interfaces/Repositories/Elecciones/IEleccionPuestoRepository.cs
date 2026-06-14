using eVote360Pro.Core.Domain.Entities.Elecciones;
using eVote360Pro.Core.Domain.Entities.Puestos;

namespace eVote360Pro.Core.Domain.Interfaces.Repositories.Elecciones
{
    public interface IEleccionPuestoRepository : IGenericRepository<EleccionPuesto>
    {
        Task<List<PuestoElectivo>> GetPuestosByEleccionAsync(int eleccionId);
    }
}
