using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Elecciones;
using eVote360Pro.Core.Application.Dtos.Puestos;
using eVote360Pro.Core.Application.Interfaces.Elecciones;
using eVote360Pro.Core.Domain.Entities.Elecciones;
using eVote360Pro.Core.Domain.Enums;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Elecciones;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Partidos;
using eVote360Pro.Core.Domain.Interfaces.Repositories.Puestos;

namespace eVote360Pro.Core.Application.Services.Elecciones
{
    public class EleccionService : IEleccionService
    {
        private readonly IEleccionRepository _eleccionRepository;
        private readonly IEleccionPuestoRepository _eleccionPuestoRepository;
        private readonly IPuestoElectivoRepository _puestoElectivoRepository;
        private readonly IPartidoPoliticoRepository _partidoPoliticoRepository;
        private readonly IAsignacionCandidatoPuestoRepository _asignacionRepository;
        private readonly IVotoRepository _votoRepository;
        private readonly IMapper _mapper;

        public EleccionService(
            IEleccionRepository eleccionRepository,
            IEleccionPuestoRepository eleccionPuestoRepository,
            IPuestoElectivoRepository puestoElectivoRepository,
            IPartidoPoliticoRepository partidoPoliticoRepository,
            IAsignacionCandidatoPuestoRepository asignacionRepository,
            IVotoRepository votoRepository,
            IMapper mapper)
        {
            _eleccionRepository = eleccionRepository;
            _eleccionPuestoRepository = eleccionPuestoRepository;
            _puestoElectivoRepository = puestoElectivoRepository;
            _partidoPoliticoRepository = partidoPoliticoRepository;
            _asignacionRepository = asignacionRepository;
            _votoRepository = votoRepository;
            _mapper = mapper;
        }

        public async Task<EleccionDto?> AddAsync(SaveEleccionDto dto)
        {
            try
            {
                if (await _eleccionRepository.HasActiveElectionAsync()) return null;

                var errors = await ValidateConfigurationAsync();
                if (errors.Count > 0) return null;

                Eleccion entity = _mapper.Map<Eleccion>(dto);
                entity.Estado = EstadoEleccion.Pendiente;
                entity.IsActive = true;
                var returnEntity = await _eleccionRepository.AddAsync(entity);
                return returnEntity == null ? null : _mapper.Map<EleccionDto>(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<EleccionDto?> UpdateAsync(SaveEleccionDto dto)
        {
            try
            {
                var existing = await _eleccionRepository.GetByIdAsync(dto.Id);
                if (existing == null || existing.Estado != EstadoEleccion.Pendiente) return null;

                existing.Nombre = dto.Nombre;
                existing.Fecha = dto.Fecha;
                existing.UpdatedAt = DateTime.UtcNow;
                var returnEntity = await _eleccionRepository.UpdateAsync(existing.Id, existing);
                return returnEntity == null ? null : _mapper.Map<EleccionDto>(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _eleccionRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<EleccionDto?> GetById(int id)
        {
            var entity = await _eleccionRepository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<EleccionDto>(entity);
        }

        public async Task<List<EleccionDto>> GetAll()
        {
            var list = await _eleccionRepository.GetAllAsync();
            return list
                .OrderByDescending(e => e.Estado == EstadoEleccion.Activa)
                .ThenByDescending(e => e.Fecha)
                .Select(e => _mapper.Map<EleccionDto>(e))
                .ToList();
        }

        public async Task<EleccionDto?> GetActiveElectionAsync()
        {
            var entity = await _eleccionRepository.GetActiveElectionAsync();
            return entity == null ? null : _mapper.Map<EleccionDto>(entity);
        }

        public async Task<bool> HasActiveElectionAsync()
        {
            return await _eleccionRepository.HasActiveElectionAsync();
        }

        public async Task<bool> ActivateAsync(int id)
        {
            if (await _eleccionRepository.HasActiveElectionAsync()) return false;

            var eleccion = await _eleccionRepository.GetByIdAsync(id);
            if (eleccion == null || eleccion.Estado != EstadoEleccion.Pendiente) return false;

            var errors = await ValidateConfigurationAsync();
            if (errors.Count > 0) return false;

            var puestosActivos = (await _puestoElectivoRepository.GetAllAsync()).Where(p => p.IsActive).ToList();
            foreach (var puesto in puestosActivos)
            {
                await _eleccionPuestoRepository.AddAsync(new EleccionPuesto
                {
                    EleccionId = eleccion.Id,
                    PuestoElectivoId = puesto.Id,
                    IsActive = true
                });
            }

            eleccion.Estado = EstadoEleccion.Activa;
            eleccion.UpdatedAt = DateTime.UtcNow;
            await _eleccionRepository.UpdateAsync(eleccion.Id, eleccion);
            return true;
        }

        public async Task<bool> FinalizeAsync(int id)
        {
            var eleccion = await _eleccionRepository.GetByIdAsync(id);
            if (eleccion == null || eleccion.Estado != EstadoEleccion.Activa) return false;

            eleccion.Estado = EstadoEleccion.Finalizada;
            eleccion.UpdatedAt = DateTime.UtcNow;
            await _eleccionRepository.UpdateAsync(eleccion.Id, eleccion);
            return true;
        }

        public async Task<bool> HasPuestosAsync(int eleccionId)
        {
            var puestos = await _eleccionPuestoRepository.GetPuestosByEleccionAsync(eleccionId);
            return puestos.Count > 0;
        }

        public async Task<bool> HasVotesAsync(int eleccionId)
        {
            var votos = await _votoRepository.GetAllAsync();
            return votos.Any(v => v.EleccionId == eleccionId);
        }

        public async Task<List<int>> GetAniosDisponiblesAsync()
        {
            var elecciones = await _eleccionRepository.GetAllAsync();
            return elecciones.Select(e => e.Fecha.Year).Distinct().OrderByDescending(y => y).ToList();
        }

        public async Task<List<ResumenElectoralDto>> GetResumenByAnioAsync(int anio)
        {
            var elecciones = await _eleccionRepository.GetAllAsync();
            var result = new List<ResumenElectoralDto>();
            foreach (var e in elecciones.Where(x => x.Fecha.Year == anio))
            {
                var resumen = await GetResumenEleccionAsync(e.Id);
                if (resumen != null) result.Add(resumen);
            }
            return result;
        }

        public async Task<ResumenElectoralDto?> GetResumenEleccionAsync(int eleccionId)
        {
            var eleccion = await _eleccionRepository.GetByIdAsync(eleccionId);
            if (eleccion == null) return null;

            var asignaciones = (await _asignacionRepository.GetAllAsync()).Where(a => a.IsActive).ToList();
            var partidosParticipantes = asignaciones.Select(a => a.PartidoPoliticoId).Distinct().Count();
            var candidatosReales = asignaciones.Select(a => a.CandidatoId).Distinct().Count();
            var votos = (await _votoRepository.GetAllAsync()).Where(v => v.EleccionId == eleccionId).ToList();
            var puestos = await _eleccionPuestoRepository.GetPuestosByEleccionAsync(eleccionId);

            return new ResumenElectoralDto
            {
                Id = eleccion.Id,
                Nombre = eleccion.Nombre,
                Fecha = eleccion.Fecha,
                CantidadPartidos = partidosParticipantes,
                CantidadCandidatos = candidatosReales,
                CantidadCiudadanosVotaron = votos.Count,
                CantidadPuestos = puestos.Count
            };
        }

        public async Task<List<PuestoVotacionDto>> GetPuestosVotacionAsync(int eleccionId, Dictionary<int, int?> selecciones)
        {
            var puestos = await _eleccionPuestoRepository.GetPuestosByEleccionAsync(eleccionId);
            if (puestos.Count == 0)
            {
                puestos = (await _puestoElectivoRepository.GetAllAsync()).Where(p => p.IsActive).ToList();
            }

            var asignaciones = (await _asignacionRepository.GetAllAsync()).Where(a => a.IsActive).ToList();
            var result = new List<PuestoVotacionDto>();

            foreach (var puesto in puestos)
            {
                var delPuesto = asignaciones.Where(a => a.PuestoElectivoId == puesto.Id).ToList();
                result.Add(new PuestoVotacionDto
                {
                    Id = puesto.Id,
                    Nombre = puesto.Nombre,
                    CantidadPartidos = delPuesto.Select(a => a.PartidoPoliticoId).Distinct().Count(),
                    CantidadCandidatos = delPuesto.Select(a => a.CandidatoId).Distinct().Count(),
                    Seleccionado = selecciones.ContainsKey(puesto.Id)
                });
            }

            return result;
        }

        public async Task<PuestoElectivoDto?> GetPuestoByIdAsync(int puestoId)
        {
            var entity = await _puestoElectivoRepository.GetByIdAsync(puestoId);
            return entity == null ? null : _mapper.Map<PuestoElectivoDto>(entity);
        }

        public async Task<List<CandidatoVotacionDto>> GetCandidatosByPuestoAndEleccionAsync(int puestoId, int eleccionId)
        {
            var asignaciones = await _asignacionRepository.GetAllWithIncludeAsync(["Candidato", "PartidoPolitico"]);
            var delPuesto = asignaciones
                .Where(a => a.PuestoElectivoId == puestoId && a.IsActive && a.Candidato.IsActive && a.PartidoPolitico.IsActive)
                .ToList();

            return delPuesto.Select(a => new CandidatoVotacionDto
            {
                Id = a.CandidatoId,
                AsignacionId = a.Id,
                Nombre = a.Candidato.Nombre,
                Apellido = a.Candidato.Apellido,
                Foto = a.Candidato.Foto ?? string.Empty,
                PartidoNombre = a.PartidoPolitico.Nombre,
                PartidoSiglas = a.PartidoPolitico.Siglas,
                PartidoLogo = a.PartidoPolitico.Logo ?? string.Empty
            }).ToList();
        }

        public async Task<List<ResultadoPuestoDto>> GetResultadosAsync(int eleccionId)
        {
            var puestos = await _eleccionPuestoRepository.GetPuestosByEleccionAsync(eleccionId);
            var votos = (await _votoRepository.GetAllWithIncludeAsync(["DetallesVoto", "DetallesVoto.Candidato", "DetallesVoto.PuestoElectivo"]))
                .Where(v => v.EleccionId == eleccionId)
                .ToList();

            var asignaciones = await _asignacionRepository.GetAllWithIncludeAsync(["Candidato", "PartidoPolitico"]);
            var resultados = new List<ResultadoPuestoDto>();

            foreach (var puesto in puestos)
            {
                var detallesPuesto = votos.SelectMany(v => v.DetallesVoto).Where(d => d.PuestoElectivoId == puesto.Id).ToList();
                var totalVotos = detallesPuesto.Count;
                var opciones = new List<ResultadoOpcionDto>();

                var asignacionesPuesto = asignaciones.Where(a => a.PuestoElectivoId == puesto.Id && a.IsActive).ToList();
                foreach (var grupo in asignacionesPuesto.GroupBy(a => a.CandidatoId))
                {
                    var asignacion = grupo.First();
                    var votosOpcion = detallesPuesto.Count(d => d.CandidatoId == asignacion.CandidatoId && !d.VotoNulo);
                    opciones.Add(new ResultadoOpcionDto
                    {
                        PuestoNombre = puesto.Nombre,
                        CandidatoNombre = $"{asignacion.Candidato.Nombre} {asignacion.Candidato.Apellido}",
                        PartidoNombre = asignacion.PartidoPolitico.Nombre,
                        CantidadVotos = votosOpcion,
                        Porcentaje = totalVotos == 0 ? 0 : Math.Round((decimal)votosOpcion / totalVotos * 100, 2)
                    });
                }

                var votosNinguno = detallesPuesto.Count(d => d.VotoNulo);
                opciones.Add(new ResultadoOpcionDto
                {
                    PuestoNombre = puesto.Nombre,
                    CandidatoNombre = "Ninguno",
                    PartidoNombre = "No aplica",
                    CantidadVotos = votosNinguno,
                    Porcentaje = totalVotos == 0 ? 0 : Math.Round((decimal)votosNinguno / totalVotos * 100, 2)
                });

                opciones = opciones.OrderByDescending(o => o.CantidadVotos).ToList();
                var maxVotos = opciones.Max(o => o.CantidadVotos);
                var ganadores = opciones.Where(o => o.CantidadVotos == maxVotos && maxVotos > 0).ToList();
                var hayEmpate = ganadores.Count > 1;

                foreach (var op in opciones)
                {
                    op.HayEmpate = hayEmpate;
                    op.EsGanador = !hayEmpate && op.CantidadVotos == maxVotos && maxVotos > 0;
                }

                resultados.Add(new ResultadoPuestoDto
                {
                    PuestoElectivoId = puesto.Id,
                    PuestoNombre = puesto.Nombre,
                    ExisteEmpate = hayEmpate,
                    Opciones = opciones
                });
            }

            return resultados;
        }

        public async Task<List<string>> ValidateConfigurationAsync()
        {
            var errors = new List<string>();
            var puestosActivos = (await _puestoElectivoRepository.GetAllAsync()).Where(p => p.IsActive).ToList();
            var partidosActivos = (await _partidoPoliticoRepository.GetAllAsync()).Where(p => p.IsActive).ToList();
            var asignaciones = (await _asignacionRepository.GetAllAsync()).Where(a => a.IsActive).ToList();

            if (puestosActivos.Count == 0)
            {
                errors.Add("No hay puestos electivos activos para realizar una elección.");
                return errors;
            }

            if (partidosActivos.Count < 2)
            {
                errors.Add("No hay suficientes partidos políticos para realizar una elección.");
                return errors;
            }

            foreach (var partido in partidosActivos)
            {
                var faltantes = puestosActivos
                    .Where(p => !asignaciones.Any(a => a.PartidoPoliticoId == partido.Id && a.PuestoElectivoId == p.Id))
                    .Select(p => p.Nombre)
                    .ToList();

                if (faltantes.Count > 0)
                {
                    errors.Add($"El partido político {partido.Nombre} ({partido.Siglas}) no tiene candidatos activos asignados para los siguientes puestos electivos: {string.Join(", ", faltantes)}.");
                }
            }

            return errors;
        }
    }
}
