using eVote360Pro.Core.Application.Dtos.Elecciones;

namespace eVote360Pro.Core.Application.Interfaces.Elecciones
{
    public interface IVotoService
    {
        Task<string?> ValidateSelectionsAsync(int ciudadanoId, int eleccionId, Dictionary<int, int?> seleccionesPorPuesto);
        Task<bool> RegistrarVotoAsync(int ciudadanoId, int eleccionId, Dictionary<int, int?> seleccionesPorPuesto);
        Task<List<VotacionResumenItemDto>> BuildResumenAsync(int eleccionId, Dictionary<int, int?> seleccionesPorPuesto);
    }
}
