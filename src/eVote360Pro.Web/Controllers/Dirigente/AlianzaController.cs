using AutoMapper;
using eVote360Pro.Core.Application.Dtos.Partidos;
using eVote360Pro.Core.Application.Dtos.Alianzas;
using eVote360Pro.Core.Application.Interfaces.Alianzas;
using eVote360Pro.Core.Application.Interfaces.Elecciones;
using eVote360Pro.Core.Application.Interfaces.Partidos;
using eVote360Pro.Core.Application.ViewModels.Alianzas;
using eVote360Pro.Core.Application.ViewModels.Shared;
using eVote360Pro.Core.Domain.Enums;
using eVote360Pro.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVote360Pro.Web.Controllers.Dirigente
{
    [Authorize(Policy = "DirigentePolitico")]
    public class AlianzaController : Controller
    {
        private readonly ISolicitudAlianzaService _solicitudAlianzaService;
        private readonly IAlianzaPoliticaService _alianzaPoliticaService;
        private readonly IPartidoPoliticoService _partidoPoliticoService;
        private readonly IEleccionService _eleccionService;
        private readonly IMapper _mapper;

        public AlianzaController(
            ISolicitudAlianzaService solicitudAlianzaService,
            IAlianzaPoliticaService alianzaPoliticaService,
            IPartidoPoliticoService partidoPoliticoService,
            IEleccionService eleccionService,
            IMapper mapper)
        {
            _solicitudAlianzaService = solicitudAlianzaService;
            _alianzaPoliticaService = alianzaPoliticaService;
            _partidoPoliticoService = partidoPoliticoService;
            _eleccionService = eleccionService;
            _mapper = mapper;
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

            var vm = new AlianzasIndexViewModel
            {
                HayEleccionActiva = await _eleccionService.HasActiveElectionAsync(),
                SolicitudesPendientes = _mapper.Map<List<SolicitudAlianzaViewModel>>(
                    await _solicitudAlianzaService.GetPendingRequestsByReceiverAsync(partidoId.Value)),
                SolicitudesEnviadas = _mapper.Map<List<SolicitudAlianzaViewModel>>(
                    await _solicitudAlianzaService.GetRequestsBySenderAsync(partidoId.Value)),
                AlianzasVigentes = _mapper.Map<List<AlianzaPoliticaViewModel>>(
                    await _alianzaPoliticaService.GetVigentesByPartidoAsync(partidoId.Value))
            };

            ViewBag.PartidoId = partidoId.Value;
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            if (!partidoId.HasValue) return RedirectToAction("Login", "Account");

            var partido = await _partidoPoliticoService.GetByIdAsync(partidoId.Value);
            if (partido == null || !partido.IsActive)
            {
                TempData["Error"] = "No puede crear solicitudes de alianza porque su partido político se encuentra inactivo.";
                return RedirectToAction(nameof(Index));
            }

            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede crear una solicitud de alianza mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Partidos = await GetPartidosDisponiblesAsync(partidoId.Value);
            return View(new SaveSolicitudAlianzaViewModel { PartidoSolicitanteId = partidoId.Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveSolicitudAlianzaViewModel model)
        {
            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            if (!partidoId.HasValue) return RedirectToAction("Login", "Account");

            var partido = await _partidoPoliticoService.GetByIdAsync(partidoId.Value);
            if (partido == null || !partido.IsActive)
            {
                TempData["Error"] = "No puede crear solicitudes de alianza porque su partido político se encuentra inactivo.";
                return RedirectToAction(nameof(Index));
            }

            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede crear una solicitud de alianza mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            model.PartidoSolicitanteId = partidoId.Value;
            if (model.PartidoReceptorId == partidoId.Value)
            {
                ModelState.AddModelError(string.Empty, "No puede crear una solicitud de alianza hacia su propio partido político.");
            }

            var partidoReceptor = await _partidoPoliticoService.GetByIdAsync(model.PartidoReceptorId);
            if (partidoReceptor == null || !partidoReceptor.IsActive)
            {
                ModelState.AddModelError(string.Empty, "No puede crear una solicitud de alianza con un partido político inactivo.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Partidos = await GetPartidosDisponiblesAsync(partidoId.Value);
                return View(model);
            }

            if (await _solicitudAlianzaService.HasActiveAllianceAsync(partidoId.Value, model.PartidoReceptorId))
            {
                ModelState.AddModelError(string.Empty, "Ya existe una alianza vigente con este partido político.");
                ViewBag.Partidos = await GetPartidosDisponiblesAsync(partidoId.Value);
                return View(model);
            }

            if (await _solicitudAlianzaService.HasPendingRequestAsync(partidoId.Value, model.PartidoReceptorId))
            {
                ModelState.AddModelError(string.Empty, "Ya existe una solicitud de alianza pendiente entre estos partidos.");
                ViewBag.Partidos = await GetPartidosDisponiblesAsync(partidoId.Value);
                return View(model);
            }

            await _solicitudAlianzaService.AddAsync(_mapper.Map<SaveSolicitudAlianzaDto>(model));
            TempData["Success"] = "Solicitud de alianza creada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Aceptar(int id)
        {
            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            if (!partidoId.HasValue) return RedirectToAction("Login", "Account");

            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede aceptar una solicitud de alianza mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var solicitud = await _solicitudAlianzaService.GetById(id);
            if (solicitud == null || solicitud.PartidoReceptorId != partidoId.Value)
            {
                TempData["Error"] = "No tiene permisos para responder esta solicitud de alianza.";
                return RedirectToAction(nameof(Index));
            }

            var partidoSolicitante = await _partidoPoliticoService.GetByIdAsync(solicitud.PartidoSolicitanteId);
            var mensaje = $"¿Está seguro que desea aceptar la alianza con el partido {partidoSolicitante?.Nombre} ({partidoSolicitante?.Siglas})?";

            return View("Confirm", new ConfirmActionViewModel
            {
                Title = "Aceptar solicitud de alianza",
                Message = mensaje,
                Controller = "Alianza",
                PostAction = "AceptarConfirm",
                EntityId = id,
                ConfirmButtonClass = "btn-success"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AceptarConfirm(int id)
        {
            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            if (!partidoId.HasValue) return RedirectToAction("Login", "Account");

            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede aceptar una solicitud de alianza mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var ok = await _solicitudAlianzaService.AceptarAsync(id, partidoId.Value);
            TempData[ok ? "Success" : "Error"] = ok
                ? "Solicitud de alianza aceptada correctamente."
                : "No fue posible aceptar la solicitud de alianza.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Rechazar(int id)
        {
            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            if (!partidoId.HasValue) return RedirectToAction("Login", "Account");

            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede rechazar una solicitud de alianza mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var solicitud = await _solicitudAlianzaService.GetById(id);
            if (solicitud == null || solicitud.PartidoReceptorId != partidoId.Value)
            {
                TempData["Error"] = "No tiene permisos para responder esta solicitud de alianza.";
                return RedirectToAction(nameof(Index));
            }

            var partidoSolicitante = await _partidoPoliticoService.GetByIdAsync(solicitud.PartidoSolicitanteId);
            var mensaje = $"¿Está seguro que desea rechazar la alianza con el partido {partidoSolicitante?.Nombre} ({partidoSolicitante?.Siglas})?";

            return View("Confirm", new ConfirmActionViewModel
            {
                Title = "Rechazar solicitud de alianza",
                Message = mensaje,
                Controller = "Alianza",
                PostAction = "RechazarConfirm",
                EntityId = id,
                ConfirmButtonClass = "btn-danger"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RechazarConfirm(int id)
        {
            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            if (!partidoId.HasValue) return RedirectToAction("Login", "Account");

            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede rechazar una solicitud de alianza mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var ok = await _solicitudAlianzaService.RechazarAsync(id, partidoId.Value);
            TempData[ok ? "Success" : "Error"] = ok
                ? "Solicitud de alianza rechazada correctamente."
                : "No fue posible rechazar la solicitud de alianza.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EliminarSolicitud(int id)
        {
            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            if (!partidoId.HasValue) return RedirectToAction("Login", "Account");

            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede eliminar una solicitud de alianza mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var solicitud = await _solicitudAlianzaService.GetById(id);
            if (solicitud == null || solicitud.PartidoSolicitanteId != partidoId.Value)
            {
                TempData["Error"] = "No tiene permisos para eliminar esta solicitud de alianza.";
                return RedirectToAction(nameof(Index));
            }

            if (solicitud.Estado == EstadoSolicitudAlianza.Aceptada)
            {
                TempData["Error"] = "No se puede eliminar una solicitud aceptada porque ya generó una alianza vigente. Para terminarla debe eliminar la alianza desde el listado de alianzas vigentes.";
                return RedirectToAction(nameof(Index));
            }

            var partidoDestino = await _partidoPoliticoService.GetByIdAsync(solicitud.PartidoReceptorId);
            var mensaje = $"¿Está seguro que desea eliminar la solicitud de alianza con el partido {partidoDestino?.Nombre} ({partidoDestino?.Siglas})?";

            return View("Confirm", new ConfirmActionViewModel
            {
                Title = "Eliminar solicitud de alianza",
                Message = mensaje,
                Controller = "Alianza",
                PostAction = "EliminarSolicitudConfirm",
                EntityId = id,
                ConfirmButtonClass = "btn-danger"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarSolicitudConfirm(int id)
        {
            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            if (!partidoId.HasValue) return RedirectToAction("Login", "Account");

            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede eliminar una solicitud de alianza mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var solicitud = await _solicitudAlianzaService.GetById(id);
            if (solicitud == null || solicitud.PartidoSolicitanteId != partidoId.Value)
            {
                TempData["Error"] = "No tiene permisos para eliminar esta solicitud de alianza.";
                return RedirectToAction(nameof(Index));
            }

            if (solicitud.Estado == EstadoSolicitudAlianza.Aceptada)
            {
                TempData["Error"] = "No se puede eliminar una solicitud aceptada porque ya generó una alianza vigente. Para terminarla debe eliminar la alianza desde el listado de alianzas vigentes.";
                return RedirectToAction(nameof(Index));
            }

            await _solicitudAlianzaService.DeleteAsync(id);
            TempData["Success"] = "Solicitud de alianza eliminada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EliminarAlianza(int id)
        {
            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            if (!partidoId.HasValue) return RedirectToAction("Login", "Account");

            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede eliminar una alianza política mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var alianza = await _alianzaPoliticaService.GetById(id);
            if (alianza == null)
            {
                TempData["Error"] = "La alianza política seleccionada no existe o ya fue eliminada.";
                return RedirectToAction(nameof(Index));
            }

            var partidoAliadoId = alianza.Partido1Id == partidoId.Value ? alianza.Partido2Id : alianza.Partido1Id;
            var partidoAliado = await _partidoPoliticoService.GetByIdAsync(partidoAliadoId);
            var mensaje = $"¿Está seguro que desea eliminar la alianza política con el partido {partidoAliado?.Nombre} ({partidoAliado?.Siglas})?";

            return View("Confirm", new ConfirmActionViewModel
            {
                Title = "Eliminar alianza política",
                Message = mensaje,
                Controller = "Alianza",
                PostAction = "EliminarAlianzaConfirm",
                EntityId = id,
                ConfirmButtonClass = "btn-danger"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarAlianzaConfirm(int id)
        {
            var partidoId = DirigenteHelper.GetPartidoPoliticoId(User);
            if (!partidoId.HasValue) return RedirectToAction("Login", "Account");

            if (await _eleccionService.HasActiveElectionAsync())
            {
                TempData["Error"] = "No se puede eliminar una alianza política mientras exista una elección activa.";
                return RedirectToAction(nameof(Index));
            }

            var ok = await _alianzaPoliticaService.EliminarAlianzaAsync(id, partidoId.Value);
            TempData[ok ? "Success" : "Error"] = ok
                ? "Alianza política eliminada correctamente."
                : "No se puede eliminar esta alianza porque existen candidatos aliados asignados entre estos partidos. Primero deben eliminarse las asignaciones correspondientes desde el módulo Asignar candidato a puesto.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<List<PartidoPoliticoDto>> GetPartidosDisponiblesAsync(int partidoId)
        {
            var partidos = await _partidoPoliticoService.GetAllActiveAsync();
            var disponibles = new List<PartidoPoliticoDto>();
            foreach (var p in partidos.Where(p => p.Id != partidoId))
            {
                if (await _solicitudAlianzaService.HasActiveAllianceAsync(partidoId, p.Id)) continue;
                if (await _solicitudAlianzaService.HasPendingRequestAsync(partidoId, p.Id)) continue;
                disponibles.Add(p);
            }
            return disponibles;
        }
    }
}
