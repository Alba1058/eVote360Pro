using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Partidos;
using eVote360Pro.Core.Application.Interfaces.Alianzas;
using eVote360Pro.Core.Application.Interfaces.Elecciones;
using eVote360Pro.Core.Application.Interfaces.Partidos;
using eVote360Pro.Core.Application.Interfaces.Puestos;
using eVote360Pro.Core.Application.ViewModels.Partidos;
using eVote360Pro.Core.Application.ViewModels.Shared;
using eVote360Pro.Core.Domain.Enums;
using eVote360Pro.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eVote360Pro.Web.Controllers.Dirigente
{
    [Authorize(Policy = "DirigentePolitico")]
    public class AsignacionCandidatoController : Controller
    {
        private readonly IAsignacionCandidatoPuestoService _asignacionService;
        private readonly ICandidatoService _candidatoService;
        private readonly IPuestoElectivoService _puestoElectivoService;
        private readonly ISolicitudAlianzaService _solicitudAlianzaService;
        private readonly IPartidoPoliticoService _partidoPoliticoService;
        private readonly IEleccionService _eleccionService;
        private readonly IMapper _mapper;

        public AsignacionCandidatoController(
            IAsignacionCandidatoPuestoService asignacionService,
            ICandidatoService candidatoService,
            IPuestoElectivoService puestoElectivoService,
            ISolicitudAlianzaService solicitudAlianzaService,
            IPartidoPoliticoService partidoPoliticoService,
            IEleccionService eleccionService,
            IMapper mapper)
        {
            _asignacionService = asignacionService;
            _candidatoService = candidatoService;
            _puestoElectivoService = puestoElectivoService;
            _solicitudAlianzaService = solicitudAlianzaService;
            _partidoPoliticoService = partidoPoliticoService;
            _eleccionService = eleccionService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            if (!partidoId.HasValue) return RedirectToAction("Login", "Account");

            ViewBag.HayEleccionActiva = await _eleccionService.HasActiveElectionAsync();
            var asignaciones = await _asignacionService.GetByPartidoPoliticoAsync(partidoId.Value);
            var vm = _mapper.Map<List<AsignacionCandidatoPuestoViewModel>>(asignaciones);
            foreach (var item in vm)
            {
                var candidato = await _candidatoService.GetByIdAsync(item.CandidatoId);
                if (candidato != null && candidato.PartidoPoliticoId != partidoId.Value)
                {
                    var partidoOrigen = await _partidoPoliticoService.GetByIdAsync(candidato.PartidoPoliticoId);
                    item.PartidoOrigenNombre = partidoOrigen?.Nombre ?? string.Empty;
                }
                else
                {
                    item.PartidoOrigenNombre = item.NombrePartido;
                }
            }
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            if (!partidoId.HasValue) return RedirectToAction("Login", "Account");

            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede asignar candidatos a puestos mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            await LoadCreateSelectsAsync(partidoId.Value);
            return View(new SaveAsignacionCandidatoPuestoViewModel
            {
                PartidoPoliticoId = partidoId.Value,
                TipoCandidatura = TipoCandidatura.Propio,
                IsActive = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveAsignacionCandidatoPuestoViewModel model)
        {
            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            if (!partidoId.HasValue) return RedirectToAction("Login", "Account");

            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede asignar candidatos a puestos mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            model.PartidoPoliticoId = partidoId.Value;
            if (!ModelState.IsValid)
            {
                await LoadCreateSelectsAsync(partidoId.Value);
                return View(model);
            }

            var (ok, error, tipo) = await ValidateAsignacionAsync(partidoId.Value, model.CandidatoId, model.PuestoElectivoId);
            if (!ok)
            {
                ModelState.AddModelError(string.Empty, error!);
                await LoadCreateSelectsAsync(partidoId.Value);
                return View(model);
            }

            model.TipoCandidatura = tipo;
            var dto = _mapper.Map<SaveAsignacionCandidatoPuestoDto>(model);
            var result = await _asignacionService.AddAsync(dto);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "No fue posible crear la asignación.");
                await LoadCreateSelectsAsync(partidoId.Value);
                return View(model);
            }

            TempData["Success"] = "Asignación creada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            if (!partidoId.HasValue) return RedirectToAction("Login", "Account");

            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede eliminar una asignación mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var asignacion = await _asignacionService.GetById(id);
            if (asignacion == null || asignacion.PartidoPoliticoId != partidoId.Value)
            {
                TempData["Error"] = "No tiene permisos para eliminar esta asignación.";
                return RedirectToAction(nameof(Index));
            }

            return View("Confirm", new ConfirmActionViewModel
            {
                Title = "Eliminar asignación de candidato",
                Message = "¿Está seguro que desea desvincular este candidato de este puesto electivo?",
                Controller = "AsignacionCandidato",
                PostAction = "DeleteConfirm",
                EntityId = id,
                ConfirmButtonClass = "btn-danger"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            if (!partidoId.HasValue) return RedirectToAction("Login", "Account");

            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede eliminar una asignación mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var asignacion = await _asignacionService.GetById(id);
            if (asignacion == null || asignacion.PartidoPoliticoId != partidoId.Value)
            {
                TempData["Error"] = "No tiene permisos para eliminar esta asignación.";
                return RedirectToAction(nameof(Index));
            }

            if (asignacion == null)
            {
                TempData["Error"] = "La asignación seleccionada no existe o ya fue eliminada.";
                return RedirectToAction(nameof(Index));
            }

            await _asignacionService.DeleteAsync(id);
            TempData["Success"] = "Asignación eliminada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCreateSelectsAsync(int partidoId)
        {
            var candidatos = new List<SelectListItem>();
            var puestos = new List<SelectListItem>();

            var propios = await _candidatoService.GetByPartidoPoliticoAsync(partidoId);
            foreach (var c in propios.Where(c => c.IsActive))
            {
                if (await _asignacionService.CandidateAssignedToPartyAsync(c.Id, partidoId)) continue;
                candidatos.Add(new SelectListItem($"{c.Nombre} {c.Apellido} (Propio)", c.Id.ToString()));
            }

            var alianzas = await _solicitudAlianzaService.GetAll();
            var partidosAliados = new HashSet<int>();
            foreach (var solicitud in alianzas.Where(s => s.Estado == EstadoSolicitudAlianza.Aceptada))
            {
                if (solicitud.PartidoSolicitanteId == partidoId)
                    partidosAliados.Add(solicitud.PartidoReceptorId);
                else if (solicitud.PartidoReceptorId == partidoId)
                    partidosAliados.Add(solicitud.PartidoSolicitanteId);
            }

            foreach (var aliadoId in partidosAliados)
            {
                var candidatosAliado = await _candidatoService.GetByPartidoPoliticoAsync(aliadoId);
                var asignacionesOrigen = await _asignacionService.GetByPartidoPoliticoAsync(aliadoId);
                foreach (var c in candidatosAliado.Where(c => c.IsActive))
                {
                    if (await _asignacionService.CandidateAssignedToPartyAsync(c.Id, partidoId)) continue;
                    var origen = asignacionesOrigen.FirstOrDefault(a => a.CandidatoId == c.Id);
                    if (origen == null) continue;
                    var partidoOrigen = await _partidoPoliticoService.GetByIdAsync(aliadoId);
                    candidatos.Add(new SelectListItem(
                        $"{c.Nombre} {c.Apellido} (Aliado - {partidoOrigen?.Siglas}) - {origen.NombrePuesto}",
                        c.Id.ToString()));
                }
            }

            var puestosActivos = await _puestoElectivoService.GetAllActiveAsync();
            foreach (var p in puestosActivos)
            {
                if (await _asignacionService.PositionOccupiedInPartyAsync(p.Id, partidoId)) continue;
                puestos.Add(new SelectListItem(p.Nombre, p.Id.ToString()));
            }

            ViewBag.Candidatos = candidatos;
            ViewBag.Puestos = puestos;
        }

        private async Task<(bool ok, string? error, TipoCandidatura tipo)> ValidateAsignacionAsync(int partidoId, int candidatoId, int puestoId)
        {
            var candidato = await _candidatoService.GetByIdAsync(candidatoId);
            if (candidato == null || !candidato.IsActive)
                return (false, "Debe seleccionar un candidato válido.", TipoCandidatura.Propio);

            if (await _asignacionService.CandidateAssignedToPartyAsync(candidatoId, partidoId))
                return (false, "Este candidato ya está asignado a un puesto dentro del partido.", TipoCandidatura.Propio);

            if (await _asignacionService.PositionOccupiedInPartyAsync(puestoId, partidoId))
                return (false, "Este puesto electivo ya tiene un candidato asignado dentro del partido.", TipoCandidatura.Propio);

            if (candidato.PartidoPoliticoId == partidoId)
                return (true, null, TipoCandidatura.Propio);

            if (!await _solicitudAlianzaService.HasActiveAllianceAsync(partidoId, candidato.PartidoPoliticoId))
                return (false, "No existe una alianza vigente con el partido de este candidato.", TipoCandidatura.Aliado);

            var asignacionesOrigen = await _asignacionService.GetByPartidoPoliticoAsync(candidato.PartidoPoliticoId);
            var asignacionOrigen = asignacionesOrigen.FirstOrDefault(a => a.CandidatoId == candidatoId);
            if (asignacionOrigen == null)
                return (false, "Este candidato aliado no tiene un puesto asignado en su partido de origen.", TipoCandidatura.Aliado);

            if (asignacionOrigen.PuestoElectivoId != puestoId)
                return (false, "Este candidato en su partido de origen aspira a un puesto diferente al seleccionado.", TipoCandidatura.Aliado);

            return (true, null, TipoCandidatura.Aliado);
        }
    }
}
