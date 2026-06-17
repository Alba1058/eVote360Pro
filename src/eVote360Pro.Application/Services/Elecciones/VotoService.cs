using eVote360Pro.Core.Application.Dtos.Elecciones;
using eVote360Pro.Core.Application.Interfaces.Elecciones;
using eVote360Pro.Core.Domain.Entities.Elecciones;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Ciudadania;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Elecciones;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Partidos;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Puestos;

namespace eVote360Pro.Core.Application.Services.Elecciones
{
    public class VotoService : IVotoService
    {
        private readonly IVotoRepository _votoRepository;
        private readonly IDetalleVotoRepository _detalleVotoRepository;
        private readonly ICiudadanoRepository _ciudadanoRepository;
        private readonly IEleccionRepository _eleccionRepository;
        private readonly IEleccionPuestoRepository _eleccionPuestoRepository;
        private readonly IAsignacionCandidatoPuestoRepository _asignacionRepository;
        private readonly IPuestoElectivoRepository _puestoElectivoRepository;

        public VotoService(
            IVotoRepository votoRepository,
            IDetalleVotoRepository detalleVotoRepository,
            ICiudadanoRepository ciudadanoRepository,
            IEleccionRepository eleccionRepository,
            IEleccionPuestoRepository eleccionPuestoRepository,
            IAsignacionCandidatoPuestoRepository asignacionRepository,
            IPuestoElectivoRepository puestoElectivoRepository)
        {
            _votoRepository = votoRepository;
            _detalleVotoRepository = detalleVotoRepository;
            _ciudadanoRepository = ciudadanoRepository;
            _eleccionRepository = eleccionRepository;
            _eleccionPuestoRepository = eleccionPuestoRepository;
            _asignacionRepository = asignacionRepository;
            _puestoElectivoRepository = puestoElectivoRepository;
        }

        public async Task<string?> ValidateSelectionsAsync(int ciudadanoId, int eleccionId, Dictionary<int, int?> seleccionesPorPuesto)
        {
            var ciudadano = await _ciudadanoRepository.GetByIdAsync(ciudadanoId);
            if (ciudadano == null)
                return "No existe un ciudadano registrado con este número de documento.";

            if (!ciudadano.IsActive)
                return "Este ciudadano se encuentra inactivo y no puede participar en el proceso de votación.";

            if (await _ciudadanoRepository.HasVotedInElectionAsync(ciudadanoId, eleccionId))
                return "Ya ha ejercido su derecho al voto.";

            var eleccion = await _eleccionRepository.GetByIdAsync(eleccionId);
            if (eleccion == null || eleccion.Estado != Domain.Enums.EstadoEleccion.Activa)
                return "No hay ningún proceso electoral en estos momentos.";

            var puestosValidos = (await _eleccionPuestoRepository.GetPuestosByEleccionAsync(eleccionId))
                .Where(p => p.IsActive)
                .Select(p => p.Id)
                .ToHashSet();

            var asignacionesValidas = (await _asignacionRepository.GetAllAsync())
                .Where(a => a.IsActive)
                .ToList();

            foreach (var seleccion in seleccionesPorPuesto)
            {
                if (!puestosValidos.Contains(seleccion.Key))
                    return "Existe una selección que no corresponde a un puesto electivo válido de la elección activa.";

                if (seleccion.Value is > 0)
                {
                    var candidatoValido = asignacionesValidas.Any(a =>
                        a.PuestoElectivoId == seleccion.Key &&
                        a.CandidatoId == seleccion.Value.Value);

                    if (!candidatoValido)
                        return "Existe una selección que no corresponde a un candidato válido para el puesto electivo seleccionado.";
                }
            }

            return null;
        }

        public async Task<bool> RegistrarVotoAsync(int ciudadanoId, int eleccionId, Dictionary<int, int?> seleccionesPorPuesto)
        {
            try
            {
                var validationError = await ValidateSelectionsAsync(ciudadanoId, eleccionId, seleccionesPorPuesto);
                if (validationError != null)
                    return false;

                var voto = new Voto
                {
                    CiudadanoId = ciudadanoId,
                    EleccionId = eleccionId,
                    FechaVoto = DateTime.Now,
                    IsActive = true
                };

                voto = await _votoRepository.AddAsync(voto);
                if (voto == null) return false;

                foreach (var seleccion in seleccionesPorPuesto)
                {
                    var detalle = new DetalleVoto
                    {
                        VotoId = voto.Id,
                        PuestoElectivoId = seleccion.Key,
                        CandidatoId = seleccion.Value is > 0 ? seleccion.Value : null,
                        VotoNulo = seleccion.Value is null or <= 0,
                        IsActive = true
                    };
                    await _detalleVotoRepository.AddAsync(detalle);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<VotacionResumenItemDto>> BuildResumenAsync(int eleccionId, Dictionary<int, int?> seleccionesPorPuesto)
        {
            var asignaciones = await _asignacionRepository.GetAllWithIncludeAsync(["Candidato", "PartidoPolitico"]);
            var puestos = await _puestoElectivoRepository.GetAllAsync();
            var resumen = new List<VotacionResumenItemDto>();

            foreach (var seleccion in seleccionesPorPuesto)
            {
                var puesto = puestos.FirstOrDefault(p => p.Id == seleccion.Key);
                if (puesto == null) continue;

                if (seleccion.Value is null or <= 0)
                {
                    resumen.Add(new VotacionResumenItemDto
                    {
                        PuestoNombre = puesto.Nombre,
                        Seleccion = "Ninguno",
                        Partido = "No aplica"
                    });
                    continue;
                }

                var asignacion = asignaciones.FirstOrDefault(a => a.CandidatoId == seleccion.Value && a.PuestoElectivoId == seleccion.Key);
                if (asignacion == null) continue;

                resumen.Add(new VotacionResumenItemDto
                {
                    PuestoNombre = puesto.Nombre,
                    Seleccion = $"{asignacion.Candidato.Nombre} {asignacion.Candidato.Apellido}",
                    Partido = asignacion.PartidoPolitico.Nombre
                });
            }

            return resumen;
        }
    }
}
