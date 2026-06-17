using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Ciudadania;
using eVote360Pro.Core.Application.Interfaces.Ciudadania;
using eVote360Pro.Core.Application.Interfaces.Elecciones;
using eVote360Pro.Core.Application.ViewModels.Ciudadania;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.Web.Controllers.Administrador
{
    [Authorize(Policy = "Administrador")]
    public class CiudadanoController : Controller
    {
        private readonly ICiudadanoService _ciudadanoService;
        private readonly IEleccionService _eleccionService;
        private readonly IMapper _mapper;

        public CiudadanoController(ICiudadanoService ciudadanoService, IEleccionService eleccionService, IMapper mapper)
        {
            _ciudadanoService = ciudadanoService;
            _eleccionService = eleccionService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.HayEleccionActiva = await _eleccionService.HasActiveElectionAsync();
            return View(_mapper.Map<List<CiudadanoViewModel>>(await _ciudadanoService.GetAllAsync()));
        }

        public async Task<IActionResult> Create()
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede crear un ciudadano mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            return View(new SaveCiudadanoViewModel { IsActive = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveCiudadanoViewModel model)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede crear un ciudadano mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid) return View(model);
            if (await _ciudadanoService.ExistsByNumeroDocumentoAsync(model.NumeroDocumento))
            {
                ModelState.AddModelError(string.Empty, "Ya existe un ciudadano registrado con este número de documento de identidad.");
                return View(model);
            }
            if (await _ciudadanoService.ExistsByCorreoAsync(model.CorreoElectronico))
            {
                ModelState.AddModelError(string.Empty, "Ya existe un ciudadano registrado con este correo electrónico.");
                return View(model);
            }
            await _ciudadanoService.AddAsync(_mapper.Map<SaveCiudadanoDto>(model));
            TempData["Success"] = "Ciudadano creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede editar un ciudadano mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            var dto = await _ciudadanoService.GetByIdAsync(id);
            if (dto == null) return NotFound();
            var vm = _mapper.Map<SaveCiudadanoViewModel>(dto);
            vm.BloquearDocumento = await _ciudadanoService.HasParticipatedInElectionAsync(id);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SaveCiudadanoViewModel model)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede editar un ciudadano mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid) return View(model);
            if (await _ciudadanoService.ExistsByNumeroDocumentoAsync(model.NumeroDocumento, model.Id))
            {
                ModelState.AddModelError(string.Empty, "Ya existe un ciudadano registrado con este número de documento de identidad.");
                return View(model);
            }
            if (await _ciudadanoService.ExistsByCorreoAsync(model.CorreoElectronico, model.Id))
            {
                ModelState.AddModelError(string.Empty, "Ya existe un ciudadano registrado con este correo electrónico.");
                return View(model);
            }
            await _ciudadanoService.UpdateAsync(_mapper.Map<SaveCiudadanoDto>(model));
            TempData["Success"] = "Ciudadano actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Activate(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede activar un ciudadano mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            var ciudadano = await _ciudadanoService.GetByIdAsync(id);
            if (ciudadano == null) return NotFound();
            if (ciudadano.IsActive)
            {
                TempData["Error"] = "Este ciudadano ya se encuentra activo.";
                return RedirectToAction(nameof(Index));
            }
            return View("Activate", ciudadano);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateConfirmed(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede activar un ciudadano mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            var ciudadano = await _ciudadanoService.GetByIdAsync(id);
            if (ciudadano == null) return NotFound();
            if (ciudadano.IsActive)
            {
                TempData["Error"] = "Este ciudadano ya se encuentra activo.";
                return RedirectToAction(nameof(Index));
            }
            await _ciudadanoService.ActivateAsync(id);
            TempData["Success"] = "Ciudadano activado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Deactivate(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede desactivar un ciudadano mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            var ciudadano = await _ciudadanoService.GetByIdAsync(id);
            if (ciudadano == null) return NotFound();
            if (!ciudadano.IsActive)
            {
                TempData["Error"] = "Este ciudadano ya se encuentra inactivo.";
                return RedirectToAction(nameof(Index));
            }
            return View("Deactivate", ciudadano);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateConfirmed(int id)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede desactivar un ciudadano mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }
            var ciudadano = await _ciudadanoService.GetByIdAsync(id);
            if (ciudadano == null) return NotFound();
            if (!ciudadano.IsActive)
            {
                TempData["Error"] = "Este ciudadano ya se encuentra inactivo.";
                return RedirectToAction(nameof(Index));
            }
            await _ciudadanoService.DeactivateAsync(id);
            TempData["Success"] = "Ciudadano desactivado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
