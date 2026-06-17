using eVote360Pro.Core.Application.Interfaces.Alianzas;
using eVote360Pro.Core.Application.Interfaces.Partidos;
using eVote360Pro.Core.Application.ViewModels.Dirigente;
using eVote360Pro.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.Web.Controllers.Dirigente
{
    [Authorize(Policy = "DirigentePolitico")]
    public class DirigenteController : Controller
    {
        private readonly IPartidoPoliticoService _partidoPoliticoService;
        private readonly ICandidatoService _candidatoService;
        private readonly ISolicitudAlianzaService _solicitudAlianzaService;
        private readonly IAlianzaPoliticaService _alianzaPoliticaService;
        private readonly IAsignacionCandidatoPuestoService _asignacionService;

        public DirigenteController(
            IPartidoPoliticoService partidoPoliticoService,
            ICandidatoService candidatoService,
            ISolicitudAlianzaService solicitudAlianzaService,
            IAlianzaPoliticaService alianzaPoliticaService,
            IAsignacionCandidatoPuestoService asignacionService)
        {
            _partidoPoliticoService = partidoPoliticoService;
            _candidatoService = candidatoService;
            _solicitudAlianzaService = solicitudAlianzaService;
            _alianzaPoliticaService = alianzaPoliticaService;
            _asignacionService = asignacionService;
        }

        public async Task<IActionResult> Index()
        {
            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            if (!partidoId.HasValue)
            {
                TempData["Error"] = "No tiene un partido político asignado. Por favor, póngase en contacto con un administrador.";
                return RedirectToAction("Login", "Account");
            }

            var partido = await _partidoPoliticoService.GetByIdAsync(partidoId.Value);
            if (partido == null || !partido.IsActive)
            {
                TempData["Error"] = "El partido político asignado a este usuario se encuentra inactivo.";
                return RedirectToAction("Login", "Account");
            }

            var candidatos = await _candidatoService.GetByPartidoPoliticoAsync(partidoId.Value);
            var solicitudesPendientes = await _solicitudAlianzaService.GetPendingRequestsByReceiverAsync(partidoId.Value);
            var alianzas = await _alianzaPoliticaService.GetVigentesByPartidoAsync(partidoId.Value);
            var asignaciones = await _asignacionService.GetByPartidoPoliticoAsync(partidoId.Value);

            var vm = new HomeDirigenteViewModel
            {
                PartidoNombre = partido.Nombre,
                PartidoSiglas = partido.Siglas,
                PartidoLogo = partido.Logo,
                CandidatosActivos = candidatos.Count(c => c.IsActive),
                CandidatosInactivos = candidatos.Count(c => !c.IsActive),
                AlianzasPoliticas = alianzas.Count,
                SolicitudesPendientes = solicitudesPendientes.Count,
                CandidatosAsignados = asignaciones.Count
            };

            return View(vm);
        }
    }
}
