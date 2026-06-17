using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Elecciones;
using eVote360Pro.Core.Application.Interfaces.Elecciones;
using eVote360Pro.Core.Application.Interfaces.Puestos;
using eVote360Pro.Core.Application.ViewModels.Elecciones;
using eVote360Pro.Core.Application.ViewModels.Shared;
using eVote360Pro.Core.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.Web.Controllers.Administrador
{
    [Authorize(Policy = "Administrador")]
    public class EleccionController : Controller
    {
        private readonly IEleccionService _eleccionService;
        private readonly IPuestoElectivoService _puestoElectivoService;
        private readonly IMapper _mapper;

        public EleccionController(IEleccionService eleccionService, IPuestoElectivoService puestoElectivoService, IMapper mapper)
        {
            _eleccionService = eleccionService;
            _puestoElectivoService = puestoElectivoService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var elecciones = await _eleccionService.GetAllAsync();
            var vm = new List<EleccionViewModel>();

            foreach (var e in elecciones)
            {
                var item = _mapper.Map<EleccionViewModel>(e);
                var resumen = await _eleccionService.GetResumenEleccionAsync(e.Id);
                if (resumen != null)
                {
                    item.CantidadPartidos = resumen.CantidadPartidos;
                    item.CantidadPuestos = resumen.CantidadPuestos;
                    item.CantidadCiudadanosVotaron = resumen.CantidadCiudadanosVotaron;
                }
                vm.Add(item);
            }

            ViewBag.HayEleccionActiva = elecciones.Any(e => e.Estado == EstadoEleccion.Activa);
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede crear una nueva elección mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            return View(new SaveEleccionViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveEleccionViewModel model)
        {
            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede crear una nueva elección mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid) return View(model);

            var errores = await _eleccionService.ValidateConfigurationAsync();
            if (errores.Count > 0)
            {
                foreach (var error in errores)
                    ModelState.AddModelError(string.Empty, error);
                return View(model);
            }

            await _eleccionService.AddAsync(_mapper.Map<SaveEleccionDto>(model));
            TempData["Success"] = "Elección creada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Activate(int id)
        {
            var eleccion = await _eleccionService.GetByIdAsync(id);
            if (eleccion == null) return NotFound();

            return View("Confirm", new ConfirmActionViewModel
            {
                Title = "Activar elección",
                Message = "¿Está seguro que desea activar esta elección?",
                Controller = "Eleccion",
                PostAction = "ActivateConfirm",
                EntityId = id,
                ConfirmButtonClass = "btn-success"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateConfirm(int id)
        {
            var ok = await _eleccionService.ActivateAsync(id);
            TempData[ok ? "Success" : "Error"] = ok
                ? "Elección activada correctamente."
                : "No se puede activar esta elección porque la configuración electoral actual no está completa.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Finalize(int id)
        {
            var eleccion = await _eleccionService.GetByIdAsync(id);
            if (eleccion == null) return NotFound();

            return View("Confirm", new ConfirmActionViewModel
            {
                Title = "Finalizar elección",
                Message = "¿Está seguro que desea finalizar esta elección?",
                Controller = "Eleccion",
                PostAction = "FinalizeConfirm",
                EntityId = id,
                ConfirmButtonClass = "btn-danger"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinalizeConfirm(int id)
        {
            var ok = await _eleccionService.FinalizeAsync(id);
            TempData[ok ? "Success" : "Error"] = ok
                ? "Elección finalizada correctamente."
                : "Solo se pueden finalizar elecciones activas.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Resultados(int id)
        {
            var eleccion = await _eleccionService.GetByIdAsync(id);
            if (eleccion == null || eleccion.Estado != EstadoEleccion.Finalizada)
                return RedirectToAction(nameof(Index));

            var resultados = await _eleccionService.GetResultadosAsync(id);
            var vm = new ResultadosEleccionViewModel
            {
                EleccionId = eleccion.Id,
                EleccionNombre = eleccion.Nombre,
                Fecha = eleccion.Fecha,
                Puestos = _mapper.Map<List<ResultadoPuestoViewModel>>(resultados)
            };

            return View(vm);
        }
    }
}
