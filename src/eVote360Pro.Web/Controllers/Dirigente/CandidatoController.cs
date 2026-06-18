using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Partidos;
using eVote360Pro.Core.Application.Interfaces.Elecciones;
using eVote360Pro.Core.Application.Interfaces.Partidos;
using eVote360Pro.Core.Application.ViewModels.Partidos;
using eVote360Pro.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.Web.Controllers.Dirigente
{
    [Authorize(Policy = "DirigentePolitico")]
    public class CandidatoController : Controller
    {
        private readonly ICandidatoService _candidatoService;
        private readonly IPartidoPoliticoService _partidoPoliticoService;
        private readonly IEleccionService _eleccionService;
        private readonly IAsignacionCandidatoPuestoService _asignacionService;
        private readonly IMapper _mapper;

        public CandidatoController(
            ICandidatoService candidatoService,
            IPartidoPoliticoService partidoPoliticoService,
            IEleccionService eleccionService,
            IAsignacionCandidatoPuestoService asignacionService,
            IMapper mapper)
        {
            _candidatoService = candidatoService;
            _partidoPoliticoService = partidoPoliticoService;
            _eleccionService = eleccionService;
            _asignacionService = asignacionService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var partidoId = await GetPartidoIdOrRedirectAsync();
            if (partidoId == null) return RedirectToAction("Login", "Account");

            ViewBag.HayEleccionActiva = await _eleccionService.HasActiveElectionAsync();
            var candidatos = await _candidatoService.GetByPartidoPoliticoAsync(partidoId.Value);
            var asignaciones = await _asignacionService.GetByPartidoPoliticoAsync(partidoId.Value);
            var vm = _mapper.Map<List<CandidatoViewModel>>(candidatos);
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            var partidoId = await GetPartidoIdOrRedirectAsync();
            if (partidoId == null) return RedirectToAction("Login", "Account");

            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede crear un candidato mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            return View(new SaveCandidatoViewModel { IsActive = false, PartidoPoliticoId = partidoId.Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveCandidatoViewModel model, IFormFile? fotoFile)
        {
            var partidoId = await GetPartidoIdOrRedirectAsync();
            if (partidoId == null) return RedirectToAction("Login", "Account");

            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede crear un candidato mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            if (fotoFile == null)
                ModelState.AddModelError(string.Empty, "La foto del candidato debe ser una imagen válida.");
            if (!ModelState.IsValid) return View(model);

            model.PartidoPoliticoId = partidoId.Value;
            var dto = _mapper.Map<SaveCandidatoDto>(model);
            dto.Foto = await FileManager.UploadAsync(fotoFile, "candidatos") ?? string.Empty;
            await _candidatoService.AddAsync(dto);
            TempData["Success"] = "Candidato creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var partidoId = await GetPartidoIdOrRedirectAsync();
            if (partidoId == null) return RedirectToAction("Login", "Account");

            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede editar un candidato mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var dto = await _candidatoService.GetByIdAsync(id);
            if (dto == null || dto.PartidoPoliticoId != partidoId.Value)
            {
                TempData["Error"] = "No tiene permisos para modificar este candidato.";
                return RedirectToAction(nameof(Index));
            }

            var vm = _mapper.Map<SaveCandidatoViewModel>(dto);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SaveCandidatoViewModel model, IFormFile? fotoFile)
        {
            var partidoId = await GetPartidoIdOrRedirectAsync();
            if (partidoId == null) return RedirectToAction("Login", "Account");

            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede editar un candidato mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var dtoActual = await _candidatoService.GetByIdAsync(model.Id);
            if (dtoActual == null || dtoActual.PartidoPoliticoId != partidoId.Value)
            {
                TempData["Error"] = "No tiene permisos para modificar este candidato.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid) return View(model);

            var dto = _mapper.Map<SaveCandidatoDto>(model);
            dto.PartidoPoliticoId = partidoId.Value;
            dto.Foto = await FileManager.UploadAsync(fotoFile, "candidatos", model.Foto) ?? model.Foto;
            await _candidatoService.UpdateAsync(dto);
            TempData["Success"] = "Candidato actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Activate(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede activar un candidato mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            var dto = await _candidatoService.GetByIdAsync(id);
            if (dto == null || dto.PartidoPoliticoId != partidoId)
            {
                TempData["Error"] = "No tiene permisos para activar este candidato.";
                return RedirectToAction(nameof(Index));
            }

            if (dto.IsActive)
            {
                TempData["Error"] = "Este candidato ya se encuentra activo.";
                return RedirectToAction(nameof(Index));
            }

            var partido = await _partidoPoliticoService.GetByIdAsync(dto.PartidoPoliticoId);
            if (partido == null || !partido.IsActive)
            {
                TempData["Error"] = "No se puede activar este candidato porque su partido político se encuentra inactivo.";
                return RedirectToAction(nameof(Index));
            }

            return View("Activate", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateConfirmed(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede activar un candidato mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            var dto = await _candidatoService.GetByIdAsync(id);
            if (dto == null || dto.PartidoPoliticoId != partidoId)
            {
                TempData["Error"] = "No tiene permisos para activar este candidato.";
                return RedirectToAction(nameof(Index));
            }

            if (dto.IsActive)
            {
                TempData["Error"] = "Este candidato ya se encuentra activo.";
                return RedirectToAction(nameof(Index));
            }

            var partido = await _partidoPoliticoService.GetByIdAsync(dto.PartidoPoliticoId);
            if (partido == null || !partido.IsActive)
            {
                TempData["Error"] = "No se puede activar este candidato porque su partido político se encuentra inactivo.";
                return RedirectToAction(nameof(Index));
            }

            await _candidatoService.ActivateAsync(id);
            TempData["Success"] = "Candidato activado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede desactivar un candidato mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            var dto = await _candidatoService.GetByIdAsync(id);
            if (dto == null || dto.PartidoPoliticoId != partidoId)
            {
                TempData["Error"] = "No tiene permisos para desactivar este candidato.";
                return RedirectToAction(nameof(Index));
            }

            if (!dto.IsActive)
            {
                TempData["Error"] = "Este candidato ya se encuentra inactivo.";
                return RedirectToAction(nameof(Index));
            }

            if (await _candidatoService.IsAssignedToPositionAsync(id))
            {
                TempData["Error"] = "No se puede desactivar este candidato porque está asignado a un puesto electivo.";
                return RedirectToAction(nameof(Index));
            }

            return View("Deactivate", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateConfirmed(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede desactivar un candidato mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            var dto = await _candidatoService.GetByIdAsync(id);
            if (dto == null || dto.PartidoPoliticoId != partidoId)
            {
                TempData["Error"] = "No tiene permisos para desactivar este candidato.";
                return RedirectToAction(nameof(Index));
            }

            if (!dto.IsActive)
            {
                TempData["Error"] = "Este candidato ya se encuentra inactivo.";
                return RedirectToAction(nameof(Index));
            }

            if (await _candidatoService.IsAssignedToPositionAsync(id))
            {
                TempData["Error"] = "No se puede desactivar este candidato porque está asignado a un puesto electivo.";
                return RedirectToAction(nameof(Index));
            }

            await _candidatoService.DeactivateAsync(id);
            TempData["Success"] = "Candidato desactivado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<int?> GetPartidoIdOrRedirectAsync()
        {
            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            if (!partidoId.HasValue)
            {
                TempData["Error"] = "No puede crear candidatos porque no tiene un partido político asignado.";
                return null;
            }

            var partido = await _partidoPoliticoService.GetByIdAsync(partidoId.Value);
            if (partido == null || !partido.IsActive)
            {
                TempData["Error"] = "No puede crear candidatos porque el partido político asignado se encuentra inactivo.";
                return null;
            }
            return partidoId;
        }
    }
}
