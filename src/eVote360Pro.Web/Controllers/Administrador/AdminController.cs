using AutoMapper;
using eVote360Pro.Core.Application.Interfaces.Elecciones;
using eVote360Pro.Core.Application.ViewModels.Administrador;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.Web.Controllers.Administrador
{
    [Authorize(Policy = "Administrador")]
    public class AdminController : Controller
    {
        private readonly IEleccionService _eleccionService;
        private readonly IMapper _mapper;

        public AdminController(IEleccionService eleccionService, IMapper mapper)
        {
            _eleccionService = eleccionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var anios = await _eleccionService.GetAniosDisponiblesAsync();
            var vm = new HomeAdminViewModel
            {
                AniosDisponibles = anios,
                AnioSeleccionado = anios.FirstOrDefault()
            };

            if (vm.AnioSeleccionado > 0)
                vm.Resumenes = _mapper.Map<List<ResumenElectoralViewModel>>(await _eleccionService.GetResumenByAnioAsync(vm.AnioSeleccionado));

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(HomeAdminViewModel model)
        {
            model.AniosDisponibles = await _eleccionService.GetAniosDisponiblesAsync();

            if (model.AnioSeleccionado <= 0)
            {
                ModelState.AddModelError(string.Empty, "Debe seleccionar un año para consultar el resumen electoral.");
                return View(model);
            }

            var resumenes = await _eleccionService.GetResumenByAnioAsync(model.AnioSeleccionado);
            if (resumenes.Count == 0)
                ModelState.AddModelError(string.Empty, "No existen elecciones registradas para el año seleccionado.");

            model.Resumenes = _mapper.Map<List<ResumenElectoralViewModel>>(resumenes);
            return View(model);
        }
    }
}
