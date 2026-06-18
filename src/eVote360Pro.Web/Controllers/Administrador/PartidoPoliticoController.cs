using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Partidos;
using eVote360Pro.Core.Application.Interfaces.Elecciones;
using eVote360Pro.Core.Application.Interfaces.Partidos;
using eVote360Pro.Core.Application.ViewModels.Partidos;
using eVote360Pro.Web.Helpers;
using eVote360Pro.Core.Application.ViewModels.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.Web.Controllers.Administrador
{
    [Authorize(Policy = "Administrador")]
    public class PartidoPoliticoController : Controller
    {
        private readonly IPartidoPoliticoService _partidoPoliticoService;
        private readonly IEleccionService _eleccionService;
        private readonly IMapper _mapper;

        public PartidoPoliticoController(IPartidoPoliticoService partidoPoliticoService, IEleccionService eleccionService, IMapper mapper)
        {
            _partidoPoliticoService = partidoPoliticoService;
            _eleccionService = eleccionService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.HayEleccionActiva = await _eleccionService.HasActiveElectionAsync();
            return View(_mapper.Map<List<PartidoPoliticoViewModel>>(await _partidoPoliticoService.GetAllAsync()));
        }

        public async Task<IActionResult> Create()
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede crear un partido político mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            return View(new SavePartidoPoliticoViewModel { IsActive = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SavePartidoPoliticoViewModel model, IFormFile? logoFile)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede crear un partido político mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            if (logoFile == null)
                ModelState.AddModelError(string.Empty, "El logo del partido debe ser una imagen válida.");
            if (!ModelState.IsValid) return View(model);

            if (await _partidoPoliticoService.ExistsByNameAsync(model.Nombre))
            {
                ModelState.AddModelError(string.Empty, "Ya existe un partido político registrado con este nombre.");
                return View(model);
            }
            if (await _partidoPoliticoService.ExistsBySiglasAsync(model.Siglas))
            {
                ModelState.AddModelError(string.Empty, "Ya existe un partido político registrado con estas siglas.");
                return View(model);
            }

            var dto = _mapper.Map<SavePartidoPoliticoDto>(model);
            dto.Logo = await FileManager.UploadAsync(logoFile, "partidos") ?? string.Empty;
            await _partidoPoliticoService.AddAsync(dto);
            TempData["Success"] = "Partido político creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede editar un partido político mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            var dto = await _partidoPoliticoService.GetByIdAsync(id);
            if (dto == null) return NotFound();
            var vm = _mapper.Map<SavePartidoPoliticoViewModel>(dto);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SavePartidoPoliticoViewModel model, IFormFile? logoFile)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede editar un partido político mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid) return View(model);

            if (await _partidoPoliticoService.HasParticipatedInElectionAsync(model.Id))
            {
                var current = await _partidoPoliticoService.GetByIdAsync(model.Id);
                if (current != null &&
                    (!current.Nombre.Equals(model.Nombre.Trim(), StringComparison.OrdinalIgnoreCase)
                     || !current.Siglas.Equals(model.Siglas.Trim(), StringComparison.OrdinalIgnoreCase)
                     || (logoFile != null && logoFile.Length > 0)))
                {
                    ModelState.AddModelError(string.Empty, "No se pueden modificar los datos principales de este partido político porque ya participó en una elección.");
                    return View(model);
                }
            }

            if (await _partidoPoliticoService.ExistsByNameAsync(model.Nombre, model.Id))
            {
                ModelState.AddModelError(string.Empty, "Ya existe un partido político registrado con este nombre.");
                return View(model);
            }
            if (await _partidoPoliticoService.ExistsBySiglasAsync(model.Siglas, model.Id))
            {
                ModelState.AddModelError(string.Empty, "Ya existe un partido político registrado con estas siglas.");
                return View(model);
            }

            var dto = _mapper.Map<SavePartidoPoliticoDto>(model);
            dto.Logo = await FileManager.UploadAsync(logoFile, "partidos", model.Logo) ?? model.Logo;
            await _partidoPoliticoService.UpdateAsync(dto);
            TempData["Success"] = "Partido político actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Activate(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede activar un partido político mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var partido = await _partidoPoliticoService.GetByIdAsync(id);
            if (partido == null) return NotFound();
            if (partido.IsActive)
            {
                TempData["Error"] = "Este partido político ya se encuentra activo.";
                return RedirectToAction(nameof(Index));
            }

            return View("Confirm", new ConfirmActionViewModel
            {
                Title = "Activar partido político",
                Message = "¿Está seguro que desea activar este partido político?",
                Controller = "PartidoPolitico",
                PostAction = "ActivateConfirm",
                EntityId = id,
                ConfirmButtonClass = "btn-success"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateConfirm(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede activar un partido político mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            var partido = await _partidoPoliticoService.GetByIdAsync(id);
            if (partido == null) return NotFound();
            if (partido.IsActive)
            {
                TempData["Error"] = "Este partido político ya se encuentra activo.";
                return RedirectToAction(nameof(Index));
            }
            await _partidoPoliticoService.ActivateAsync(id);
            TempData["Success"] = "Partido político activado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede desactivar un partido político mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            var partido = await _partidoPoliticoService.GetByIdAsync(id);
            if (partido == null) return NotFound();
            if (!partido.IsActive)
            {
                TempData["Error"] = "Este partido político ya se encuentra inactivo.";
                return RedirectToAction(nameof(Index));
            }
            if (await _partidoPoliticoService.HasActiveCandidatesAsync(id))
            {
                TempData["Error"] = "No se puede desactivar este partido político porque tiene candidatos activos registrados.";
                return RedirectToAction(nameof(Index));
            }
            if (await _partidoPoliticoService.HasActiveLeadersAsync(id))
            {
                TempData["Error"] = "No se puede desactivar este partido político porque tiene un dirigente político asignado.";
                return RedirectToAction(nameof(Index));
            }

            return View("Confirm", new ConfirmActionViewModel
            {
                Title = "Desactivar partido político",
                Message = "¿Está seguro que desea desactivar este partido político?",
                Controller = "PartidoPolitico",
                PostAction = "DeactivateConfirm",
                EntityId = id,
                ConfirmButtonClass = "btn-danger"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateConfirm(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede desactivar un partido político mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            var partido = await _partidoPoliticoService.GetByIdAsync(id);
            if (partido == null) return NotFound();
            if (!partido.IsActive)
            {
                TempData["Error"] = "Este partido político ya se encuentra inactivo.";
                return RedirectToAction(nameof(Index));
            }
            if (await _partidoPoliticoService.HasActiveCandidatesAsync(id))
            {
                TempData["Error"] = "No se puede desactivar este partido político porque tiene candidatos activos registrados.";
                return RedirectToAction(nameof(Index));
            }
            if (await _partidoPoliticoService.HasActiveLeadersAsync(id))
            {
                TempData["Error"] = "No se puede desactivar este partido político porque tiene un dirigente político asignado.";
                return RedirectToAction(nameof(Index));
            }
            await _partidoPoliticoService.DeactivateAsync(id);
            TempData["Success"] = "Partido político desactivado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
