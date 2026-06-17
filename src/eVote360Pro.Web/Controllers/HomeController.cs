using eVote360Pro.Core.Application.Interfaces.Ciudadania;
using eVote360Pro.Core.Application.Interfaces.Elecciones;
using eVote360Pro.Core.Application.ViewModels.Elector;
using eVote360Pro.Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICiudadanoService _ciudadanoService;
        private readonly IEleccionService _eleccionService;

        public HomeController(ICiudadanoService ciudadanoService, IEleccionService eleccionService)
        {
            _ciudadanoService = ciudadanoService;
            _eleccionService = eleccionService;
        }

        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole(RolUsuario.Administrador.ToString()))
                    return RedirectToAction("Index", "Admin");
                if (User.IsInRole(RolUsuario.DirigentePolitico.ToString()))
                    return RedirectToAction("Index", "Dirigente");
            }

            return View(new VotarViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Votar(VotarViewModel model)
        {
            if (!ModelState.IsValid) return View("Index", model);

            var eleccionActiva = await _eleccionService.GetActiveElectionAsync();
            if (eleccionActiva == null)
            {
                ModelState.AddModelError(string.Empty, "No hay ningún proceso electoral en estos momentos.");
                return View("Index", model);
            }

            var ciudadano = await _ciudadanoService.GetByNumeroDocumentoAsync(model.NumeroDocumento.Trim());
            if (ciudadano == null)
            {
                ModelState.AddModelError(string.Empty, "No existe un ciudadano registrado con este número de documento.");
                return View("Index", model);
            }

            if (!ciudadano.IsActive)
            {
                ModelState.AddModelError(string.Empty, "Este ciudadano se encuentra inactivo y no puede participar en el proceso de votación.");
                return View("Index", model);
            }

            if (await _ciudadanoService.HasVotedInElectionAsync(ciudadano.Id, eleccionActiva.Id))
            {
                ModelState.AddModelError(string.Empty, "Ya ha ejercido su derecho al voto.");
                return View("Index", model);
            }

            return RedirectToAction("ValidarIdentidad", "Elector", new { numeroDocumento = model.NumeroDocumento.Trim(), ciudadanoId = ciudadano.Id });
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
