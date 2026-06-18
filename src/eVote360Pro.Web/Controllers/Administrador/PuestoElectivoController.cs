using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Puestos;
using eVote360Pro.Core.Application.Interfaces.Elecciones;
using eVote360Pro.Core.Application.Interfaces.Puestos;
using eVote360Pro.Core.Application.ViewModels.Puestos;
using eVote360Pro.Core.Application.ViewModels.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.Web.Controllers.Administrador
{
    [Authorize(Policy = "Administrador")]
    public class PuestoElectivoController : Controller
    {
        private readonly IPuestoElectivoService _puestoElectivoService;
        private readonly IEleccionService _eleccionService;
        private readonly IMapper _mapper;

        public PuestoElectivoController(
            IPuestoElectivoService puestoElectivoService,
            IEleccionService eleccionService,
            IMapper mapper)
        {
            _puestoElectivoService = puestoElectivoService;
            _eleccionService = eleccionService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var puestos = await _puestoElectivoService.GetAllAsync();
            ViewBag.HayEleccionActiva = await _eleccionService.HasActiveElectionAsync();
            return View(_mapper.Map<List<PuestoElectivoViewModel>>(puestos));
        }

        public async Task<IActionResult> Create()
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede crear un puesto electivo mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            return View(new SavePuestoElectivoViewModel { IsActive = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SavePuestoElectivoViewModel model)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede crear un puesto electivo mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid) return View(model);

            if (await _puestoElectivoService.ExistsByNameAsync(model.Nombre))
            {
                ModelState.AddModelError(string.Empty, "Ya existe un puesto electivo registrado con este nombre.");
                return View(model);
            }

            await _puestoElectivoService.AddAsync(_mapper.Map<SavePuestoElectivoDto>(model));
            TempData["Success"] = "Puesto electivo creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede editar un puesto electivo mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var puesto = await _puestoElectivoService.GetByIdAsync(id);
            if (puesto == null) return NotFound();

            var vm = _mapper.Map<SavePuestoElectivoViewModel>(puesto);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SavePuestoElectivoViewModel model)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede editar un puesto electivo mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid) return View(model);

            if (await _puestoElectivoService.HasParticipatedInElectionAsync(model.Id))
            {
                var current = await _puestoElectivoService.GetByIdAsync(model.Id);
                if (current != null && !current.Nombre.Equals(model.Nombre.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(string.Empty, "No se puede modificar el nombre de este puesto electivo porque ya fue utilizado en una elección.");
                    return View(model);
                }
            }

            if (await _puestoElectivoService.ExistsByNameAsync(model.Nombre, model.Id))
            {
                ModelState.AddModelError(string.Empty, "Ya existe un puesto electivo registrado con este nombre.");
                return View(model);
            }

            await _puestoElectivoService.UpdateAsync(_mapper.Map<SavePuestoElectivoDto>(model));
            TempData["Success"] = "Puesto electivo actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Activate(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede activar un puesto electivo mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var puesto = await _puestoElectivoService.GetByIdAsync(id);
            if (puesto == null) return NotFound();
            if (puesto.IsActive)
            {
                TempData["Error"] = "Este puesto electivo ya se encuentra activo.";
                return RedirectToAction(nameof(Index));
            }

            return View("Confirm", new ConfirmActionViewModel
            {
                Title = "Activar puesto electivo",
                Message = "¿Está seguro que desea activar este puesto electivo?",
                Controller = "PuestoElectivo",
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
                TempData["Error"] = "No se puede activar un puesto electivo mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var puesto = await _puestoElectivoService.GetByIdAsync(id);
            if (puesto == null) return NotFound();
            if (puesto.IsActive)
            {
                TempData["Error"] = "Este puesto electivo ya se encuentra activo.";
                return RedirectToAction(nameof(Index));
            }

            await _puestoElectivoService.ActivateAsync(id);
            TempData["Success"] = "Puesto electivo activado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede desactivar un puesto electivo mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var puesto = await _puestoElectivoService.GetByIdAsync(id);
            if (puesto == null) return NotFound();
            if (!puesto.IsActive)
            {
                TempData["Error"] = "Este puesto electivo ya se encuentra inactivo.";
                return RedirectToAction(nameof(Index));
            }

            if (await _puestoElectivoService.HasCandidatesAsync(id))
            {
                TempData["Error"] = "No se puede desactivar este puesto electivo porque tiene candidatos activos asignados.";
                return RedirectToAction(nameof(Index));
            }

            return View("Confirm", new ConfirmActionViewModel
            {
                Title = "Desactivar puesto electivo",
                Message = "¿Está seguro que desea desactivar este puesto electivo?",
                Controller = "PuestoElectivo",
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
                TempData["Error"] = "No se puede desactivar un puesto electivo mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var puesto = await _puestoElectivoService.GetByIdAsync(id);
            if (puesto == null) return NotFound();
            if (!puesto.IsActive)
            {
                TempData["Error"] = "Este puesto electivo ya se encuentra inactivo.";
                return RedirectToAction(nameof(Index));
            }

            if (await _puestoElectivoService.HasCandidatesAsync(id))
            {
                TempData["Error"] = "No se puede desactivar este puesto electivo porque tiene candidatos activos asignados.";
                return RedirectToAction(nameof(Index));
            }

            await _puestoElectivoService.DeactivateAsync(id);
            TempData["Success"] = "Puesto electivo desactivado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
